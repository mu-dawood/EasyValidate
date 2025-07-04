using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using EasyValidate.Handlers;
using System.Collections.Immutable;
using EasyValidate;
using System.Collections.Generic;
using System;
using Microsoft.CodeAnalysis.CSharp;

namespace EasyValidate
{
    [Generator]
    public class EasyValidateGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            DebuggerUtil.AttachDebugger();
            DebuggerUtil.Log("Initializing EasyValidateGenerator...");
            DebuggerUtil.Log("Starting Initialize method.");

            var compilationProvider = context.CompilationProvider;

            var candidates = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (node, _) => node is ClassDeclarationSyntax,
                    transform: static (ctx, _) => ctx.SemanticModel.GetDeclaredSymbol((ClassDeclarationSyntax)ctx.Node) as INamedTypeSymbol
                )
                .Where(static symbol => symbol != null)
                .Combine(compilationProvider);

            context.RegisterSourceOutput(candidates, static (spc, pair) =>
            {
                var (classSymbol, compilation) = pair;

                if (classSymbol is not INamedTypeSymbol classSymbolNonNull)
                    return;

                DebuggerUtil.Log($"Processing class: {classSymbolNonNull.Name}");
                GenerateValidationClass(classSymbolNonNull, compilation, spc);
            });

            DebuggerUtil.Log("Finished setting up syntax provider and compilation provider.");
            DebuggerUtil.Log("Finished Initialize method.");
        }
        private static bool IsBackingField(ISymbol symbol)
        {
            if (symbol is not IFieldSymbol field)
                return false;

            // Backing fields typically have names like "<PropertyName>k__BackingField"
            return field.Name.StartsWith("<") && field.Name.EndsWith("k__BackingField");
        }
        private static void GenerateValidationClass(INamedTypeSymbol classSymbol, Compilation compilation, SourceProductionContext context)
        {
            DebuggerUtil.Log($"Generating validation class for: {classSymbol.Name}");

            try
            {
                // Skip classes that have no properties with validation attributes
                // Check if the class has any properties with attributes derived from IValidationAttribute
                List<MemberInfo> memberInfos = [];
                var argumentHandler = new AttributeArgumentHandler();
                var members = classSymbol.GetMembers();
                Dictionary<string, string> instanceNames = [];
                foreach (var member in members)
                {
                    if (member is not IPropertySymbol && member is not IFieldSymbol)
                        continue;
                    if (IsBackingField(member)) continue; // Skip backing fields
                    bool isProperty = member is IPropertySymbol;
                    var type = isProperty ? ((IPropertySymbol)member).Type : ((IFieldSymbol)member).Type;
                    var name = member.Name;
                    var (requireNested, isCollection) = RequiresNestedValidation(type);
                    var info = new MemberInfo(name, type, isProperty, requireNested, isCollection);

                    foreach (var attr in member.GetAttributes())
                    {
                        if (attr.AttributeClass?.IsValidationAttribute(out var inputAndOutputTypes) == true)
                        {
                            var instnceDeclration = GenerateAttributeInitialization(attr, argumentHandler);
                            if (instanceNames.ContainsKey(instnceDeclration))
                            {
                                info.Attributes.Add(new AttributeInfo(attr, instanceNames[instnceDeclration], instnceDeclration, inputAndOutputTypes));
                            }
                            else
                            {
                                // Generate a unique instance name for the attribute
                                var baseName = attr.AttributeClass.Name.ToSakeCase();
                                var instanceName = baseName;
                                int suffixIndex = 0;
                                string[] suffixes = Enumerable.Range('a', 26).Select(i => ((char)i).ToString()).ToArray();
                                while (instanceNames.Values.Contains(instanceName))
                                {
                                    string suffix = suffixIndex < suffixes.Length ? suffixes[suffixIndex] : $"_{suffixIndex}";
                                    instanceName = $"{baseName}_{suffix}";
                                    suffixIndex++;
                                }
                                instanceNames[instnceDeclration] = instanceName;
                                info.Attributes.Add(new AttributeInfo(attr, instanceName, instnceDeclration, inputAndOutputTypes));
                            }
                        }
                    }
                    if (info.Attributes.Count > 0 || (info.RequireNestedValidation && IsGettingField(classSymbol, member)))
                        memberInfos.Add(info);
                }

                if (memberInfos.Count == 0)
                {
                    DebuggerUtil.Log($"Skipping class {classSymbol.Name} as it has no properties with validation attributes derived from IValidationAttribute.");
                    return;
                }
                var sb = new StringBuilder();
                var chain = new GeneratorChain()
                    .Add(new UsingImportsHandler())
                    .Add(new NamespaceHandler())
                    .Add(new ClassDeclarationHandler())
                    .Add(new ReusableInstancesHandler())
                    .Add(new ValidateMethodOverlodsHandler())
                    .Add(new ValidateMethodHandler(compilation))
                    .Add(new MemberValidationMethodHandler(compilation));

                chain.Handle(new HandlerParams(memberInfos, context, sb, classSymbol));

                var namespacePath = classSymbol.ContainingNamespace.ToDisplayString().Replace('.', '/');
                var fileName = $"{classSymbol.Name}_Validation.g.cs";
                context.AddSource($"{namespacePath}/{fileName}", SourceText.From(sb.ToString(), Encoding.UTF8));
            }
            catch (Exception ex)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor("EVGEN001", "Validation Class Generation Error", "Error generating validation class for {0}: {1}", "EasyValidate", DiagnosticSeverity.Error, true),
                   classSymbol.Locations.First(), classSymbol.Name, ex.Message));
                DebuggerUtil.Log($"Error generating validation class for {classSymbol.Name}: {ex.Message}");
                return;
            }
            DebuggerUtil.Log($"Successfully generated validation class for: {classSymbol.Name}");
        }


        private static bool IsGettingField(INamedTypeSymbol classSymbol, ISymbol member)
        {
            if (member is IPropertySymbol prop)
            {
                var syntaxRef = prop.DeclaringSyntaxReferences.FirstOrDefault();
                if (syntaxRef != null)
                {
                    var propDecl = syntaxRef.GetSyntax() as PropertyDeclarationSyntax;
                    // Expression-bodied property: public string Prop => _field;
                    if (propDecl?.ExpressionBody?.Expression is IdentifierNameSyntax idName)
                    {
                        if (classSymbol.GetMembers().OfType<IFieldSymbol>().Any(f => f.Name == idName.Identifier.Text))
                        {
                            return true;
                        }
                    }
                    // Block-bodied property: public string Prop { get { return _field; } }
                    else if (propDecl?.AccessorList != null)
                    {
                        var getter = propDecl.AccessorList.Accessors.FirstOrDefault(a => a.IsKind(SyntaxKind.GetAccessorDeclaration));
                        if (getter?.Body?.Statements.Count == 1 &&
                            getter.Body.Statements[0] is ReturnStatementSyntax ret &&
                            ret.Expression is IdentifierNameSyntax idName2 &&
                            classSymbol.GetMembers().OfType<IFieldSymbol>().Any(f => f.Name == idName2.Identifier.Text))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Generates the attribute initialization code with constructor and named arguments.
        /// </summary>
        private static string GenerateAttributeInitialization(AttributeData attr, AttributeArgumentHandler argumentHandler)
        {
            var attributeClass = attr.AttributeClass!;
            var constructorArguments = argumentHandler.FormatConstructorArguments(attr);

            // Create attribute instance with constructor arguments - use short class name since we have using directive
            var shortClassName = attributeClass.Name;

            // Handle generic attributes by adding type parameters
            if (attributeClass.IsGenericType && attributeClass.TypeArguments.Length > 0)
            {
                var typeArguments = attributeClass.TypeArguments
                    .Select(typeArg => SimplifyTypeName(typeArg.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)))
                    .ToArray();
                shortClassName = $"{shortClassName}<{string.Join(", ", typeArguments)}>";
            }

            var attributeCreation = $"new {shortClassName}({string.Join(", ", constructorArguments)})";

            // If there are named arguments (property assignments), use object initialization syntax
            if (attr.NamedArguments.Any())
            {
                // Build object initializer with named arguments
                var namedArguments = attr.NamedArguments
                    .Select(namedArg => $"{namedArg.Key} = {argumentHandler.FormatArgument(namedArg.Value)}")
                    .ToList();

                return $"{attributeCreation} {{ {string.Join(", ", namedArguments)} }}";
            }
            else
            {
                var instanceProperty = attributeClass.GetMembers("Instance")
                .OfType<IPropertySymbol>().FirstOrDefault(p => p.IsStatic && p.DeclaredAccessibility == Accessibility.Public && SymbolEqualityComparer.Default.Equals(p.Type, attributeClass));
                if (instanceProperty != null)
                {
                    // If the attribute has a static Instance property, use it
                    return $"{attributeClass.Name}.Instance";
                }
                return attributeCreation;
            }
        }

        /// <summary>
        /// Simplifies type names by removing global:: prefix and using short forms for common types.
        /// </summary>
        private static string SimplifyTypeName(string typeName)
        {
            // Remove global:: prefix
            if (typeName.StartsWith("global::"))
            {
                typeName = typeName.Substring(8);
            }

            // Map common system types to their C# keywords
            return typeName switch
            {
                "System.String" => "string",
                "System.Int32" => "int",
                "System.Int64" => "long",
                "System.Double" => "double",
                "System.Decimal" => "decimal",
                "System.Single" => "float",
                "System.Boolean" => "bool",
                "System.DateTime" => "DateTime",
                "System.Object" => "object",
                "System.Byte" => "byte",
                "System.SByte" => "sbyte",
                "System.Int16" => "short",
                "System.UInt16" => "ushort",
                "System.UInt32" => "uint",
                "System.UInt64" => "ulong",
                "System.Collections.IEnumerable" => "IEnumerable",
                _ => typeName
            };
        }



        /// <summary>
        /// Determines if a type requires nested validation.
        /// </summary>
        private static (bool requireNestedValidation, bool isCollection) RequiresNestedValidation(ITypeSymbol memberType)
        {
            if (IsCollection(memberType))
            {
                // For collections, check if the element type has validations
                var elementType = GetCollectionElementType(memberType);
                if (elementType != null && HasValidations(elementType))
                {
                    return (true, true);
                }
                return (false, true); // It's a collection but elements don't need validation
            }
            return (HasValidations(memberType), false);


        }

        /// <summary>
        /// Gets the element type of a collection.
        /// </summary>
        private static ITypeSymbol? GetCollectionElementType(ITypeSymbol type)
        {
            // Handle arrays
            if (type is IArrayTypeSymbol arrayType)
                return arrayType.ElementType;

            // Handle generic collections (IEnumerable<T>, List<T>, etc.)
            if (type is INamedTypeSymbol namedType)
            {
                // Look for IEnumerable<T> interface
                var enumerableInterface = namedType.AllInterfaces
                    .FirstOrDefault(i => i.IsGenericType &&
                                   i.ConstructedFrom.ToDisplayString() == "System.Collections.Generic.IEnumerable<T>");

                if (enumerableInterface != null && enumerableInterface.TypeArguments.Length > 0)
                {
                    return enumerableInterface.TypeArguments[0];
                }

                // If it's a generic type itself (like List<T>), check its type arguments
                if (namedType.IsGenericType && namedType.TypeArguments.Length > 0)
                {
                    // For most collections, the first type argument is the element type
                    return namedType.TypeArguments[0];
                }
            }

            return null;
        }

        private static bool HasValidations(ITypeSymbol type)
        {
            // Get all properties and fields from the type
            var properties = type.GetMembers().OfType<IPropertySymbol>();
            // Check if any properties have validation attributes
            var hasPropertiesWithValidation = properties.Any(property =>
                property.GetAttributes().Any(attr =>
                    attr.AttributeClass?.IsValidationAttribute() == true));

            if (hasPropertiesWithValidation)
            {
                return true;
            }

            var fields = type.GetMembers().OfType<IFieldSymbol>();
            // Check if any fields have validation attributes
            var hasFieldsWithValidation = fields.Any(field =>
                field.GetAttributes().Any(attr =>
                    attr.AttributeClass?.IsValidationAttribute() == true));

            return hasFieldsWithValidation;
        }

        /// <summary>
        /// Determines if a type is a collection type.
        /// </summary>
        private static bool IsCollection(ITypeSymbol type)
        {
            // Exclude string type (even though it implements IEnumerable<char>)
            if (type.SpecialType == SpecialType.System_String)
                return false;

            // Check for arrays first
            if (type is IArrayTypeSymbol)
                return true;

            // Check if it's a collection type (implements IEnumerable)
            if (type is INamedTypeSymbol namedType)
            {
                // Check if the collection type implements IEnumerable (generic or non-generic)
                bool isEnumerable = namedType.AllInterfaces.Any(i =>
                {
                    var interfaceName = i.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    return interfaceName.StartsWith("global::System.Collections.Generic.IEnumerable") ||
                           interfaceName == "global::System.Collections.IEnumerable";
                });

                return isEnumerable;
            }

            return false;
        }

    }
}

public class MemberInfo(string name, ITypeSymbol type, bool isProperty, bool requireNestedValidation, bool isCollection)
{
    public string Name { get; } = name;
    public bool IsCollection { get; } = isCollection;
    public bool RequireNestedValidation { get; } = requireNestedValidation;
    public ITypeSymbol Type { get; } = type;
    public bool IsProperty { get; } = isProperty;
    public List<AttributeInfo> Attributes { get; } = [];
}

public class AttributeInfo(AttributeData attribute, string instanceName, string instanceDeclration, ImmutableArray<InputAndOutputTypes> inputAndOutputTypes)
{
    public AttributeData Attribute => attribute;

    public string InstanceName => instanceName;
    public string InstanceDeclration => instanceDeclration;

    public ImmutableArray<InputAndOutputTypes> InputAndOutputTypes => inputAndOutputTypes;
}