using System.Collections.Generic;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Generator.Analyzers.AttributeAnalyzers
{
    /// <summary>
    /// Validates proper usage of PowerOfAttribute to ensure meaningful power validation.
    /// </summary>
    /// <docs-explanation>
    /// PowerOfAttribute validates that a number is a power of a specified base. The base must be greater than 1 
    /// for the validation to be meaningful. Using base values of 1 or 0 would result in invalid or meaningless 
    /// power calculations (1^n is always 1, and 0^n is undefined for most cases).
    /// </docs-explanation>
    /// <docs-good-example>
    /// internal class Settings
    /// {
    ///     [PowerOf(2)] // Valid: checks if value is a power of 2
    ///     internal int BufferSize { get; set; }
    /// }
    /// </docs-good-example>
    /// <docs-bad-example>
    /// internal class Settings
    /// {
    ///     [PowerOf(1)] // Invalid: 1^n is always 1
    ///     [PowerOf(0)] // Invalid: 0^n is undefined
    ///     internal int BufferSize { get; set; }
    /// }
    /// </docs-bad-example>
    /// <docs-fixes>
    /// Use a base value greater than 1|Common valid bases: 2 (binary), 3, 10 (decimal), 16 (hexadecimal)|Remove the attribute if power validation is not needed
    /// </docs-fixes>
    internal class PowerOfAttributeUsage : AttributeAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new(
            id: ErrorIds.PowerOfValueMustBeGreaterThanOne,
            title: "Invalid PowerOfAttribute Usage",
            messageFormat: "The value passed to 'PowerOfAttribute' must be greater than 1",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
        internal override ICollection<DiagnosticDescriptor> DiagnosticDescriptors => [Rule];

        internal override bool Analyze(AnalyserContext context, AttributeInfo attribute)
        {
            var attributeClass = attribute.Attribute.AttributeClass;
            if (attributeClass == null || !attributeClass.InheritsFrom("EasyValidate.Attributes.PowerOfAttribute"))
                return true; // Not a PowerOfAttribute, so no compatibility check needed.

            if (attribute.Attribute.ConstructorArguments.Length >= 1 &&
                attribute.Attribute.ConstructorArguments[0].Value is int baseValue)
            {
                if (baseValue <= 1)
                {
                    // Report diagnostic for invalid base value
                    var diagnostic = Diagnostic.Create(
                        Rule,
                        attribute.Location,
                        baseValue);
                    context.Context.ReportDiagnostic(diagnostic);
                    return false; // Invalid base value, analysis failed
                }
                return true; // Valid base value, analysis passed
            }
            else
            {
                // Report diagnostic for invalid constructor arguments
                var diagnostic = Diagnostic.Create(
                    Rule,
                    attribute.Location,
                    "unknown");
                context.Context.ReportDiagnostic(diagnostic);
                return false; // Invalid usage detected
            }
        }
    }
}
