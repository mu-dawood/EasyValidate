using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EasyValidate.Analyzers.Analyzers.AttributeUsage
{
    /// <summary>
    /// Validates proper usage of DivisibleByAttribute to prevent division by zero errors.
    /// </summary>
    /// <docs-explanation>
    /// DivisibleByAttribute validates that a number is divisible by a specified divisor. Division by zero is 
    /// undefined in mathematics, so the divisor cannot be zero. This analyzer ensures that only non-zero 
    /// divisor values are used with the DivisibleByAttribute.
    /// </docs-explanation>
    /// <docs-good-example>
    /// public class Pagination
    /// {
    ///     [DivisibleBy(5)] // Valid: checks if value is divisible by 5
    ///     public int PageSize { get; set; }
    /// }
    /// </docs-good-example>
    /// <docs-bad-example>
    /// public class Invalid
    /// {
    ///     [DivisibleBy(0)] // Invalid: cannot divide by zero
    ///     public int Value { get; set; }
    /// }
    /// </docs-bad-example>
    /// <docs-fixes>
    /// Use a non-zero divisor value|Consider using other numeric validation attributes if divisibility check is not needed|Use positive or negative non-zero values as appropriate
    /// </docs-fixes>
    public class DivisibleByAttributeUsage : IAttributeUsageProcessor
    {
        private static readonly DiagnosticDescriptor Rule = new(
            id: ErrorIds.DivisorCannotBeZero,
            title: "Invalid DivisibleByAttribute Usage",
            messageFormat: "The divisor passed to 'DivisibleByAttribute' cannot be zero",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public ICollection<DiagnosticDescriptor> DiagnosticDescriptors => [Rule];

        public void Process(SymbolAnalysisContext context, AttributeInfo attribute, ITypeSymbol memberType, string memberName)
        {
            var attributeClass = attribute.AttributeClass;
            if (attributeClass == null || !attributeClass.InheritsFrom("EasyValidate.Core.Attributes.DivisibleByAttribute"))
                return; // Not a DivisibleByAttribute, so no compatibility check needed.

            if (attribute.ConstructorArguments.Length >= 1 &&
                attribute.ConstructorArguments[0].Value is double divisor)
            {
                if (divisor == 0)
                {
                    // Report diagnostic for zero divisor
                    var diagnostic = Diagnostic.Create(
                        Rule,
                       attribute.Location,
                        divisor);
                    context.ReportDiagnostic(diagnostic);
                }
            }
            else
            {
                // Report diagnostic for invalid constructor arguments
                var diagnostic = Diagnostic.Create(
                    Rule,
                    attribute.Location,
                    "invalid arguments");
                context.ReportDiagnostic(diagnostic);
            }
        }

    }
}
