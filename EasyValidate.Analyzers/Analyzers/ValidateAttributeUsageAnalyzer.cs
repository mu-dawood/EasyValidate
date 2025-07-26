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
        private readonly DiagnosticDescriptor AsyncInterfaceDiagnostic = new(
            ErrorIds.ValidateAttributeUsageAsyncInterface, "Async Validation Interface Error", "{0}", "Usage", DiagnosticSeverity.Error, true);
        private readonly DiagnosticDescriptor SyncInterfaceDiagnostic = new(
            ErrorIds.ValidateAttributeUsageSyncInterface, "Sync Validation Interface Error", "{0}", "Usage", DiagnosticSeverity.Error, true);
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
                    AsyncInterfaceDiagnostic,
                    SyncInterfaceDiagnostic
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
                foreach (var member in members)
                {
                    ITypeSymbol memberType;
                    string memberName = member.Name;
                    ImmutableArray<AttributeData> attributes;
                    if (member is IPropertySymbol propertySymbol)
                    {
                        memberType = propertySymbol.Type;
                        attributes = propertySymbol.GetAttributes();
                    }
                    else if (member is IFieldSymbol fieldSymbol)
                    {
                        memberType = fieldSymbol.Type;
                        attributes = fieldSymbol.GetAttributes();
                    }
                    else
                        continue; // Skip if not a property or field

                    // Group attributes by Chain parameter value
                    var chainGroups = GroupAttributesByChain(context, member, attributes, memberType, memberName);
                    if (chainGroups.Any())
                        hasAsync = false;
                    foreach (var chainGroup in chainGroups)
                    {
                        var value = chainGroup.Value.AsReadOnly();
                        foreach (var processor in chainProcessors)
                        {
                            try
                            {
                                var (passed, order) = processor.Process(context, chainGroup.Key, value, memberType, context.Symbol.Name);
                                if (!passed) return;
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

                    foreach (var attribute in attributes)
                    {
                        var attributeClass = attribute.AttributeClass;
                        if (!attributeClass.IsValidationAttribute())
                            continue;
                    }
                }


                if (classType.ImplementsIAsyncValidate())
                    hasAsync = true;

                if (classType.IsCollectionOfIAsyncValidate())
                    hasAsync = true;
                if (hasAsync.HasValue)
                {
                    bool implementsAsync = classType.ImplementsIAsyncValidate();
                    bool implementsSync = classType.ImplementsIValidate();
                    if (hasAsync.Value && !implementsAsync)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(
                            AsyncInterfaceDiagnostic,
                            classType.Locations.FirstOrDefault(),
                            $"Class '{classType.Name}' must implement IAsyncValidate because async validation is required."
                        ));
                    }
                    else if (!hasAsync.Value && !implementsSync)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(
                            SyncInterfaceDiagnostic,
                            classType.Locations.FirstOrDefault(),
                            $"Class '{classType.Name}' must implement IValidate because only sync validation is required."
                        ));
                    }
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



        private Dictionary<string, List<AttributeInfo>> GroupAttributesByChain(SymbolAnalysisContext context, ISymbol symbol, ImmutableArray<AttributeData> attributes, ITypeSymbol memberType, string memberName)
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
                                              ?? symbol.Locations.FirstOrDefault()
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
                        processor.Process(context, info, memberType, memberName);
                    }
                }
            }

            return chainGroups;
        }
    }
}
