using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using EasyValidate.Handlers;
using System.Collections.Immutable;
using System.Collections.Generic;
using System;
using Microsoft.CodeAnalysis.CSharp;
using EasyValidate.Types;

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
                    transform: static (ctx, _) => ctx.SemanticModel.GetDeclaredSymbol((ClassDeclarationSyntax)ctx.Node)
                )
                .Where(static symbol => symbol != null)
                .Combine(compilationProvider);

            context.RegisterSourceOutput(candidates, static (spc, pair) =>
            {
                var (classSymbol, compilation) = pair;

                if (classSymbol is not INamedTypeSymbol classSymbolNonNull)
                    return;
                if (!classSymbol.ImplementsIAsyncValidate() && !classSymbol.ImplementsIValidate())
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
                List<ValidationTarget> targets = [];
                // Skip classes that have no properties with validation attributes
                // Check if the class has any properties with attributes derived from IValidationAttribute
                var argumentHandler = new AttributeArgumentHandler();
                var members = classSymbol.GetMembers().OrderBy(m => m is IFieldSymbol ? 0 : 1).ToList(); // Order properties first, then fields
                Dictionary<string, string> instanceNames = [];
                var memberInfos = GetMembers(members, instanceNames, argumentHandler, classSymbol);
                if (memberInfos.Count > 0)
                    targets.Add(new ValidationTarget(classSymbol, TargetType.CurretClass, memberInfos));

                foreach (var member in classSymbol.GetMembers())
                {
                    /// get parmters for methods to make validation for it
                    if (member is IMethodSymbol method)
                    {
                        var methodParameters = method.Parameters;
                        if (methodParameters.Length > 0)
                        {
                            var methodInfos = GetMembers(members, instanceNames, argumentHandler, classSymbol);
                            if (methodInfos.Count > 0)
                                targets.Add(new ValidationTarget(method, TargetType.Method, methodInfos));
                        }
                    }

                }

                if (targets.Count == 0)
                {
                    DebuggerUtil.Log($"Skipping class {classSymbol.Name} as it has no properties with validation attributes derived from IValidationAttribute.");
                    return;
                }
                var target = targets.FirstOrDefault(t => t.TargetType == TargetType.CurretClass);

                if (target != null)
                {
                    var chain = new GeneratorChain(new UsingImportsHandler())
                    .Add(new NamespaceHandler())
                    .Add(new ClassDeclarationHandler())
                    .Add(new ReusableInstancesHandler())
                    .Add(new ValidateMethodOverlodsHandler())
                    .Add(new ValidateMethodHandler(compilation))
                    .Add(new MemberValidationMethodHandler(compilation));


                    var (sb, _) = chain.Handle(new HandlerParams(target, context, classSymbol));

                    var namespacePath = classSymbol.ContainingNamespace.ToDisplayString().Replace('.', '/');
                    var fileName = $"{classSymbol.Name}_Validation.g.cs";
                    context.AddSource($"{namespacePath}/{fileName}", SourceText.From(sb.ToString(), Encoding.UTF8));

                }

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


        private static List<MemberInfo> GetMembers(List<ISymbol> members, Dictionary<string, string> instanceNames, AttributeArgumentHandler argumentHandler, INamedTypeSymbol classSymbol)
        {
            List<MemberInfo> memberInfos = [];
            foreach (var member in members)
            {
                if (member is not IPropertySymbol && member is not IFieldSymbol && member is not IParameterSymbol)
                    continue;
                if (member is not IParameterSymbol && IsBackingField(member)) continue; // Skip backing fields
                bool isProperty = member is IPropertySymbol;
                var type = isProperty ? ((IPropertySymbol)member).Type : ((IFieldSymbol)member).Type;
                var name = member.Name;

                List<AttributeInfo> attributes = [];

                foreach (var attr in member.GetAttributes())
                {
                    if (attr.AttributeClass?.IsValidationAttribute(out var inputAndOutputTypes) == true)
                    {
                        var instnceDeclration = GenerateAttributeInitialization(attr, argumentHandler);

                        var conditionalMethod = attr.NamedArguments.GetArgumentValue<string>("ConditionalMethod");
                        ConditionalMethodInfo? conditionalMethodInfo = null;
                        if (!string.IsNullOrWhiteSpace(conditionalMethod))
                        {
                            /// get conditional method by the class ref
                            var method = classSymbol.GetMembers()
                            .OfType<IMethodSymbol>()
                            .FirstOrDefault((x) => x.Name == conditionalMethod && x.Parameters.Length == 1 && x.Parameters[0].Type.InheritsFrom("EasyValidate.Core.Abstraction.IChainResult"));
                            if (method != null)
                            {
                                var (isAsync, _) = method.IsAsyncMethod();
                                conditionalMethodInfo = new ConditionalMethodInfo(conditionalMethod!, isAsync);
                            }
                        }

                        var chain = attr.GetChainValue();
                        if (instanceNames.ContainsKey(instnceDeclration))
                        {
                            attributes.Add(new AttributeInfo(attr, chain, conditionalMethodInfo, instanceNames[instnceDeclration], instnceDeclration, inputAndOutputTypes));
                        }
                        else if (!instanceNames.Values.Contains(attr.AttributeClass.Name))
                        {
                            instanceNames[instnceDeclration] = attr.AttributeClass.Name;
                            attributes.Add(new AttributeInfo(attr, chain, conditionalMethodInfo, attr.AttributeClass.Name, instnceDeclration, inputAndOutputTypes));
                        }
                        else
                        {
                            // Generate a unique instance name for the attribute
                            var baseName = attr.AttributeClass.Name;
                            var instanceName = baseName;
                            if (instanceNames.Values.Contains(instanceName) && !string.IsNullOrEmpty(chain))
                                instanceName = $"{baseName}_For_{chain}";
                            if (!instanceNames.Values.Contains(instanceName))
                            {
                                instanceNames[instnceDeclration] = instanceName;
                                attributes.Add(new AttributeInfo(attr, chain, conditionalMethodInfo, instanceName, instnceDeclration, inputAndOutputTypes));
                            }
                            else
                            {
                                int suffixIndex = 0;
                                string[] suffixes = [.. Enumerable.Range('A', 26).Select(i => ((char)i).ToString())];
                                while (instanceNames.Values.Contains(instanceName))
                                {
                                    string suffix = suffixIndex < suffixes.Length ? suffixes[suffixIndex] : $"_{suffixIndex}";
                                    instanceName = $"{instanceName}_{suffix}";
                                    suffixIndex++;
                                }
                                instanceNames[instnceDeclration] = instanceName;
                                attributes.Add(new AttributeInfo(attr, chain, conditionalMethodInfo, instanceName, instnceDeclration, inputAndOutputTypes));
                            }
                        }
                    }
                }

                // Check if nested validation is required
                NestedConfig? nestedConfig = null;
                if (type.ImplementsIAsyncValidate())
                    nestedConfig = new NestedConfig(false, true);
                else if (type.ImplementsIValidate())
                    nestedConfig = new NestedConfig(false, false);
                else if (type.IsCollectionOfIAsyncValidate())
                    nestedConfig = new NestedConfig(true, true);
                else if (type.IsCollectionOfIValidate())
                    nestedConfig = new NestedConfig(true, false);


                var info = new MemberInfo(name, attributes, type, isProperty, nestedConfig);

                if (attributes.Count > 0)
                    memberInfos.Add(info);
                else if (info.NestedConfig != null)
                {
                    var (isGtterForField, fieldName) = GetterField(classSymbol, member);
                    if (!isGtterForField || !memberInfos.Any(m => m.Name == fieldName))
                        memberInfos.Add(info);
                }
            }

            return memberInfos;
        }

        private static (bool found, string? fieldName) GetterField(INamedTypeSymbol classSymbol, ISymbol member)
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
                            return (true, idName.Identifier.Text);
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
                            return (true, idName2.Identifier.Text);
                        }
                    }
                }
            }
            return (false, null);
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

            var properties = attr.AttributeClass?
                .GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.Type.GetFullName() == "global::System.IServiceProvider" || p.GetAttributes().Any(a => a.AttributeClass.IsValidationContext()))
                .ToList() ?? [];
            // If there are named arguments (property assignments), use object initialization syntax
            if (attr.NamedArguments.Any() || properties.Any())
            {
                // Build object initializer with named arguments
                var namedArguments = attr.NamedArguments
                    .Select(namedArg => $"{namedArg.Key} = {argumentHandler.FormatArgument(namedArg.Value)}")
                    .ToList();
                foreach (var prop in properties)
                {
                    if (prop.GetAttributes().Any(a => a.AttributeClass.IsValidationContext()))
                        namedArguments.Add($"{prop.Name} = this");
                    else
                        namedArguments.Add($"{prop.Name} = serviceProvider");
                }
                return $"{attributeCreation} {{ {string.Join(", ", namedArguments)} }}";
            }
            else
            {

                // Check for static Instance property of the same type (public or non-public)
                var instanceMembers = attributeClass.GetMembers("Instance")
                      .Where(p => p.IsStatic && p.DeclaredAccessibility == Accessibility.Public);
                foreach (var instanceProperty in instanceMembers)
                {
                    ITypeSymbol? type = instanceProperty switch
                    {
                        IPropertySymbol prop => prop.Type,
                        IFieldSymbol field => field.Type,
                        _ => null
                    };

                    if (type is null)
                        continue;

                    if (SymbolEqualityComparer.Default.Equals(type, attributeClass))
                    {
                        return $"{attributeClass.Name}.Instance";
                    }
                    else if (type is INamedTypeSymbol namedType &&
                    namedType.Name == "Lazy" &&
                    namedType.Arity == 1 &&
                    namedType.TypeArguments.Length == 1 &&
                    SymbolEqualityComparer.Default.Equals(namedType.TypeArguments[0], attributeClass) &&
                    namedType.ContainingNamespace.ToDisplayString() == "System")
                    {
                        return $"{attributeClass.Name}.Instance.Value";
                    }

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

    }
}