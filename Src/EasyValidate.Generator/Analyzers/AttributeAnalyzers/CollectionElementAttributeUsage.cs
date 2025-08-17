using System.Collections.Generic;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Generator.Analyzers.AttributeAnalyzers
{
    /// <summary>
    /// Analyzer for collection element validation attributes in EasyValidate.
    /// Ensures that attributes such as <c>ContainsElement</c>, <c>NotContainElement</c>, <c>Single</c>, and <c>SingleOrNone</c>
    /// are used with element values that match the collection's generic type parameter.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This analyzer enforces type safety for collection element validation attributes. It checks that the element value
    /// provided to the attribute matches the type of the collection property. If a mismatch is detected, a diagnostic is reported.
    /// </para>
    /// <para>
    /// <b>Common diagnostics:</b>
    /// <list type="table">
    /// <item><term>COL001</term><description>Element type does not match collection type</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// public class UserList
    /// {
    ///     [ContainsElement("admin")]
    ///     public List&lt;string&gt; Roles { get; set; } // Valid
    ///
    ///     [ContainsElement(123)]
    ///     public List&lt;string&gt; Roles2 { get; set; } // Invalid: int does not match string
    /// }
    /// </code>
    /// </example>
    internal class CollectionElementAttributeUsage : AttributeAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new(
            id: ErrorIds.CollectionElementTypeMismatch,
            title: "Invalid Collection Element Attribute Usage",
            messageFormat: "The element passed to '{0}' must match the property type",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);


        internal override ICollection<DiagnosticDescriptor> DiagnosticDescriptors => [Rule];

        internal override bool Analyze(AnalyserContext context, AttributeInfo attribute)
        {
            var attributeClass = attribute.Attribute.AttributeClass;
            if (attributeClass == null || !IsCollectionElementAttribute(attributeClass))
                return true; // Not a collection element attribute, so no compatibility check needed.

            if (attribute.Attribute.ConstructorArguments.Length >= 1)
            {
                var elementType = attribute.Attribute.ConstructorArguments[0].Type;
                if (!SymbolEqualityComparer.Default.Equals(elementType, context.GetMemberType()))
                {
                    // Report diagnostic for type mismatch
                    var diagnostic = Diagnostic.Create(
                        Rule,
                        attribute.Location,
                        attributeClass.Name);
                    context.Context.ReportDiagnostic(diagnostic);
                    return false; // Incompatible usage detected
                }
                return true; // Valid usage
            }
            else
            {
                // Report diagnostic for invalid constructor arguments
                var diagnostic = Diagnostic.Create(
                    Rule,
                    attribute.Location,
                    attributeClass.Name);
                context.Context.ReportDiagnostic(diagnostic);
                return false; // Invalid usage detected
            }
        }

        private bool IsCollectionElementAttribute(INamedTypeSymbol type)
        {
            // Check if the type implements ValidationAttribute<T> and is a collection element attribute
            return type.Name.Contains("ContainsElement") ||
                    type.Name.Contains("NotContainElement") ||
                    type.Name.Contains("Single") ||
                    type.Name.Contains("SingleOrNone");
        }
    }
}
