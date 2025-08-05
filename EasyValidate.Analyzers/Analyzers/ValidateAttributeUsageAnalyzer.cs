using System.Collections.Immutable;
using EasyValidate.Analyzers.Analyzers.AttributeUsage;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EasyValidate.Analyzers.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ValidateAttributeUsageAnalyzer : DiagnosticAnalyzer
    {
        private readonly DiagnosticDescriptor ErrorDiagnostic = new(
            ErrorIds.ValidateAttributeUsageDebug, "Chain Processor Error", "{0}: Cause: {1}", "Usage", DiagnosticSeverity.Error, true);
        /// <summary>
        /// Diagnostic descriptor for reporting missing required validation interfaces or classes.
        /// </summary>
        private readonly DiagnosticDescriptor MissingTypeDiagnostic = new(
            ErrorIds.ValidateAttributeUsageMissingType,
            "Missing Validation Type Error",
            "Class '{0}' is missing required type(s): [{1}]",
            "Usage",
            DiagnosticSeverity.Error,
            true);

        /// <summary>
        /// Diagnostic descriptor for reporting public methods with validation chains (warning).
        /// </summary>
        private readonly DiagnosticDescriptor PublicMethodChainDiagnostic = new(
            ErrorIds.ValidateAttributeUsagePublicMethod,
            "Public Method Validation Chain Warning",
            "Method '{0}' is public and has validation attributes. This is allowed, but may lead to ambiguity between your original method and the generated overload. To ensure the generated method is called, pass 'null' or a ValidationConfig object as the last parameter.",
            "Usage",
            DiagnosticSeverity.Warning,
            true);


        private readonly List<IAttributeUsageProcessor> processors = [
            new PowerOfAttributeUsage(),
            new DivisibleByAttributeUsage(),
            new CollectionElementAttributeUsage(),
            new ConditionalMethodAttributeUsage(),
        ];

        private readonly List<IAttributeUsageChainProcessor> chainProcessors = [
            new ChainIncompatibilityProcessor(),
            new DuplicateAttributeInChainProcessor(),
        ];

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                var descriptors = new List<DiagnosticDescriptor>
                {
                    ErrorDiagnostic,
                    MissingTypeDiagnostic,
                    PublicMethodChainDiagnostic
                };
                foreach (var processor in processors)
                {
                    descriptors.AddRange(processor.DiagnosticDescriptors);
                }
                foreach (var processor in chainProcessors)
                {
                    descriptors.AddRange(processor.DiagnosticDescriptors);
                }
                return [.. descriptors];
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeAttributeUsage, SymbolKind.NamedType);
        }

        private void AnalyzeAttributeUsage(SymbolAnalysisContext context)
        {
            if (context.Symbol is not INamedTypeSymbol namedTypeSymbol)
                return;
            if (namedTypeSymbol.IsAbstract)
                return;

            var classType = namedTypeSymbol.TypeKind == TypeKind.Class || namedTypeSymbol.TypeKind == TypeKind.Struct
                ? namedTypeSymbol
                : null;
            if (classType == null)
                return;

            var members = classType.GetMembers();
            bool? hasAsync = null;
            try
            {
                var formattedTypes = new HashSet<string>();

                foreach (var member in members)
                {
                    ITypeSymbol memberType;
                    string memberName = member.Name;
                    ImmutableArray<AttributeData> attributes;
                    if (member is IPropertySymbol propertySymbol)
                    {
                        memberType = propertySymbol.Type;
                        attributes = propertySymbol.GetAttributes();
                        foreach (var attribute in attributes)
                        {
                            var props = attribute.AttributeClass?
                              .GetMembers()
                              .OfType<IPropertySymbol>() ?? [];
                            foreach (var prop in props)
                            {
                                if (prop.GetAttributes().Any(a => a.AttributeClass.IsValidationContext()))
                                {
                                    if (SymbolEqualityComparer.Default.Equals(prop.Type, classType)) continue;
                                    if (prop.Type.TypeKind == TypeKind.Interface)
                                    {
                                        if (!classType.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, prop.Type)))
                                            formattedTypes.Add("interface:" + prop.Type.ToNonNullable().ToDisplayString());
                                    }
                                    else if (prop.Type.TypeKind == TypeKind.Class)
                                    {
                                        if (!InheritsOrImplements(classType, prop.Type))
                                            formattedTypes.Add("class:" + prop.Type.ToNonNullable().ToDisplayString());
                                    }
                                }
                            }

                        }
                    }
                    else if (member is IFieldSymbol fieldSymbol)
                    {
                        memberType = fieldSymbol.Type;
                        attributes = fieldSymbol.GetAttributes();
                    }
                    else if (member is IMethodSymbol methodSymbol)
                    {
                        bool puplicReported = false;
                        foreach (var parameter in methodSymbol.Parameters)
                        {
                            var parmterChainGroups = GroupAttributesByChain(context, parameter, parameter.Type, parameter.GetAttributes());
                            if (parmterChainGroups.Count > 0 && !puplicReported && methodSymbol.DeclaredAccessibility.HasFlag(Accessibility.Public))
                            {
                                // Report diagnostic for public method with validation chain attributes
                                context.ReportDiagnostic(Diagnostic.Create(
                                    PublicMethodChainDiagnostic,
                                    methodSymbol.Locations.FirstOrDefault() ?? Location.None,
                                    methodSymbol.Name
                                ));
                                puplicReported = true;
                            }
                            foreach (var chainGroup in parmterChainGroups)
                            {
                                var value = chainGroup.Value.AsReadOnly();
                                foreach (var processor in chainProcessors)
                                {
                                    var (passed, order) = processor.Process(context, chainGroup.Key, value, parameter.Type, context.Symbol.Name);
                                    if (!passed) break;
                                }
                            }
                        }
                        continue;
                    }
                    else
                        continue; // Skip if not a property or field

                    // Group attributes by Chain parameter value
                    var chainGroups = GroupAttributesByChain(context, member, memberType, attributes);
                    if (!chainGroups.Any()) continue;
                    hasAsync = false;
                    foreach (var chainGroup in chainGroups)
                    {
                        var value = chainGroup.Value.AsReadOnly();
                        foreach (var processor in chainProcessors)
                        {
                            try
                            {
                                var (passed, order) = processor.Process(context, chainGroup.Key, value, memberType, context.Symbol.Name);
                                if (!passed) break;
                                if (order != null && order.Count > 0)
                                {
                                    // Check if any order item has async input/output types

                                    foreach (var info in order)
                                    {
                                        if (info.Types.IsAsync)
                                        {
                                            hasAsync = true;
                                            break;
                                        }
                                        if (string.IsNullOrEmpty(info.Info.ConditionalMethodName))
                                            continue;
                                        var method = classType.GetMembers().OfType<IMethodSymbol>()
                                               .FirstOrDefault(m => m.Name == info.Info.ConditionalMethodName);
                                        if (method != null && method.IsAsyncMethod().isAsync)
                                        {
                                            hasAsync = true;
                                            break;
                                        }
                                    }


                                }
                            }
                            catch { }
                        }
                    }

                }

                if (hasAsync.HasValue)
                {
                    bool implementsAsync = classType.ImplementsIAsyncValidate();
                    bool implementsSync = classType.ImplementsIValidate();
                    if (hasAsync.Value && !implementsAsync)
                        formattedTypes.Add("interface:EasyValidate.Core.Abstraction.IAsyncValidate");
                    if (!hasAsync.Value && !implementsSync)
                        formattedTypes.Add("interface:EasyValidate.Core.Abstraction.IValidate");
                }

                if (formattedTypes.Count > 0)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        MissingTypeDiagnostic,
                        classType.Locations.FirstOrDefault(),
                        classType.Name,
                        string.Join(", ", formattedTypes)
                    ));
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions from chain processors
                context.ReportDiagnostic(Diagnostic.Create(
                     ErrorDiagnostic,
                     context.Symbol.Locations.FirstOrDefault() ?? Location.None,
                     ex.Message,
                     ex.StackTrace ?? "No stack trace available"
                   )
                );
            }

        }

        bool InheritsOrImplements(ITypeSymbol typeSymbol, ITypeSymbol targetType)
        {
            // Check inheritance chain
            var current = typeSymbol;
            while (current != null)
            {
                if (SymbolEqualityComparer.Default.Equals(current, targetType))
                    return true;
                current = current.BaseType;
            }

            // Check implemented interfaces
            return typeSymbol.AllInterfaces.Any(i =>
                SymbolEqualityComparer.Default.Equals(i, targetType));
        }

        private Dictionary<string, List<AttributeInfo>> GroupAttributesByChain(SymbolAnalysisContext context, ISymbol member, ITypeSymbol memberType, ImmutableArray<AttributeData> attributes)
        {
            var chainGroups = new Dictionary<string, List<AttributeInfo>>();
            foreach (var attribute in attributes)
            {
                if (attribute.AttributeClass == null)
                    continue;

                if (attribute.AttributeClass.IsValidationAttribute(out var typeArgs))
                {
                    var chainValue = attribute.GetChainValue();
                    if (!chainGroups.TryGetValue(chainValue, out var list))
                    {
                        list = [];
                        chainGroups[chainValue] = list;
                    }

                    // Handle input/output types from the validation attribute interface

                    var location = attribute.ApplicationSyntaxReference?.GetSyntax()?.GetLocation()
                                              ?? member.Locations.FirstOrDefault()
                                              ?? Location.None;
                    var info = new AttributeInfo(
                        name: attribute.AttributeClass.Name,
                        fullName: attribute.AttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                        inputAndOutputTypes: [.. typeArgs],
                        conditionalMethodName: attribute.NamedArguments.GetArgumentValue<string>("ConditionalMethod"),
                        location: location,
                        constructorArguments: attribute.ConstructorArguments,
                        namedArguments: attribute.NamedArguments,
                        isOptionalNotNullAttribute: attribute.AttributeClass.IsOptionalOrNotNullAttribute(),
                        attributeClass: attribute.AttributeClass
                    );
                    list.Add(info);

                    foreach (var processor in processors)
                    {
                        processor.Process(context, info, memberType, member.Name);
                    }
                }
            }

            return chainGroups;
        }
    }
}
