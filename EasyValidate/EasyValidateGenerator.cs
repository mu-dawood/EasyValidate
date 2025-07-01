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
                int instanceCounter = 0;
                foreach (var member in members)
                {
                    if (member is not IPropertySymbol && member is not IFieldSymbol)
                        continue;
                    bool isProperty = member is IPropertySymbol;
                    var type = isProperty ? ((IPropertySymbol)member).Type : ((IFieldSymbol)member).Type;
                    var name = member.Name;
                    var info = new MemberInfo(name, type, isProperty);
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
                                var instanceName = $"{attr.AttributeClass.Name.ToLowerInvariant()}_{instanceCounter++}";
                                instanceNames[instnceDeclration] = instanceName;
                                info.Attributes.Add(new AttributeInfo(attr, instanceName, instnceDeclration, inputAndOutputTypes));
                            }
                        }
                    }
                    if (info.Attributes.Count > 0)
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
                DebuggerUtil.Log($"Error generating validation class for {classSymbol.Name}: {ex.Message}");
                return;
            }
            DebuggerUtil.Log($"Successfully generated validation class for: {classSymbol.Name}");
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

    }
}

public class MemberInfo(string name, ITypeSymbol type, bool isProperty)
{
    public string Name { get; } = name;
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