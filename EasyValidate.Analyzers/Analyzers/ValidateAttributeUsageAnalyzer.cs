using System.Collections.Immutable;
using System.Diagnostics;
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
                    ErrorDiagnostic
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

            context.RegisterSymbolAction(AnalyzeAttributeUsage, SymbolKind.Property);
            context.RegisterSymbolAction(AnalyzeAttributeUsage, SymbolKind.Field);
        }

        private void AnalyzeAttributeUsage(SymbolAnalysisContext context)
        {

            ITypeSymbol memberType;
            ImmutableArray<AttributeData> attributes;

            // Handle both properties and fields
            switch (context.Symbol)
            {
                case IPropertySymbol propertySymbol:
                    memberType = propertySymbol.Type;
                    attributes = propertySymbol.GetAttributes();
                    break;
                case IFieldSymbol fieldSymbol:
                    memberType = fieldSymbol.Type;
                    attributes = fieldSymbol.GetAttributes();
                    break;
                default:
                    return; // Not a property or field, skip
            }
            try
            {
                // Group attributes by Chain parameter value
                var chainGroups = GroupAttributesByChain(context, attributes, memberType, context.Symbol.Name);

                foreach (var chainGroup in chainGroups)
                {

                    var value = chainGroup.Value.AsReadOnly();
                    foreach (var processor in chainProcessors)
                    {
                        try
                        {
                            processor.Process(context, chainGroup.Key, value, memberType, context.Symbol.Name);
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



        private Dictionary<string, List<AttributeInfo>> GroupAttributesByChain(SymbolAnalysisContext context, ImmutableArray<AttributeData> attributes, ITypeSymbol memberType, string memberName)
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
                                              ?? context.Symbol.Locations.FirstOrDefault()
                                              ?? Location.None;
                    var info = new AttributeInfo
                    {
                        Name = attribute.AttributeClass.Name,
                        FullName = attribute.AttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                        InputAndOutputTypes = [.. typeArgs],
                        Location = location,
                        ConstructorArguments = attribute.ConstructorArguments,
                        NamedArguments = attribute.NamedArguments,
                        IsOptionalNotNullAttribute = attribute.AttributeClass.IsOptionalOrNotNullAttribute(),
                        AttributeClass = attribute.AttributeClass,
                    };
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
