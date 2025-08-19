using System.Collections.Generic;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Generator.Analyzers.AttributeAnalyzers
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
    /// internal class Pagination
    /// {
    ///     [DivisibleBy(5)] // Valid: checks if value is divisible by 5
    ///     internal int PageSize { get; set; }
    /// }
    /// </docs-good-example>
    /// <docs-bad-example>
    /// internal class Invalid
    /// {
    ///     [DivisibleBy(0)] // Invalid: cannot divide by zero
    ///     internal int Value { get; set; }
    /// }
    /// </docs-bad-example>
    /// <docs-fixes>
    /// Use a non-zero divisor value|Consider using other numeric validation attributes if divisibility check is not needed|Use positive or negative non-zero values as appropriate
    /// </docs-fixes>
    internal class DivisibleByAttributeUsage : AttributeAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new(
            id: ErrorIds.DivisibleByAttributeDivisorCannotBeZero,
            title: "Invalid DivisibleByAttribute Usage",
            messageFormat: "The divisor passed to 'DivisibleByAttribute' cannot be zero",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        internal override ICollection<DiagnosticDescriptor> DiagnosticDescriptors => [Rule];

        internal override bool Analyze(AnalyserContext context, AttributeInfo attribute)
        {
            var attributeClass = attribute.Attribute.AttributeClass;
            if (attributeClass == null || !attributeClass.InheritsFrom("EasyValidate.Attributes.DivisibleByAttribute"))
                return true; // Not a DivisibleByAttribute, so no compatibility check needed.

            if (attribute.Attribute.ConstructorArguments.Length >= 1 && attribute.Attribute.ConstructorArguments[0].Value is double divisor)
            {
                if (divisor == 0)
                {
                    // Report diagnostic for zero divisor
                    var diagnostic = Diagnostic.Create(
                        Rule,
                        attribute.Location,
                        divisor);
                    context.Context.ReportDiagnostic(diagnostic);
                    return false; // Invalid divisor, analysis failed
                }
                return true; // Valid divisor, analysis passed
            }
            else
            {
                // Report diagnostic for invalid constructor arguments
                var diagnostic = Diagnostic.Create(
                    Rule,
                    attribute.Location,
                    "invalid arguments");
                context.Context.ReportDiagnostic(diagnostic);
                return false; // Invalid usage detected
            }
        }

    }
}
