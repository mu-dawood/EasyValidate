using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EasyValidate.Analyzers.Analyzers.AttributeUsage
{
    /// <summary>
    /// Validates proper usage of collection element attributes to ensure type compatibility.
    /// </summary>
    /// <docs-explanation>
    /// Collection element attributes like ContainsElement, NotContainElement, Single, and SingleOrNone require 
    /// the element type to match the collection's element type. This analyzer ensures type safety by validating 
    /// that the element passed to these attributes is compatible with the property's collection type.
    /// </docs-explanation>
    /// <docs-good-example>
    /// public class UserList
    /// {
    ///     [ContainsElement("admin")] // Valid: string matches List<string>
    ///     public List&lt;string&gt; Roles { get; set; }
    /// }
    /// </docs-good-example>
    /// <docs-bad-example>
    /// public class UserList
    /// {
    ///     [ContainsElement(123)] // Invalid: int doesn't match List<string>
    ///     public List&lt;string&gt; Roles { get; set; }
    /// }
    /// </docs-bad-example>
    /// <docs-fixes>
    /// Ensure the element type matches the collection's generic type parameter|Use the correct data type for the element value|Consider using different validation attributes for type mismatches
    /// </docs-fixes>
    public class CollectionElementAttributeUsage : IAttributeUsageProcessor
    {
        private static readonly DiagnosticDescriptor Rule = new(
            id: ErrorIds.CollectionElementTypeMismatch,
            title: "Invalid Collection Element Attribute Usage",
            messageFormat: "The element passed to '{0}' must match the property type",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);


        public ICollection<DiagnosticDescriptor> DiagnosticDescriptors => [Rule];

        public void Process(SymbolAnalysisContext context, AttributeInfo attribute, ITypeSymbol memberType, string memberName)
        {
            var attributeClass = attribute.AttributeClass;
            if (attributeClass == null || !IsCollectionElementAttribute(attributeClass))
                return; // Not a collection element attribute, so no compatibility check needed.

            if (attribute.ConstructorArguments.Length >= 1)
            {
                var elementType = attribute.ConstructorArguments[0].Type;
                if (!SymbolEqualityComparer.Default.Equals(elementType, memberType))
                {
                    // Report diagnostic for type mismatch
                    var diagnostic = Diagnostic.Create(
                        Rule,
                       attribute.Location,
                        attributeClass.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
            else
            {
                // Report diagnostic for invalid constructor arguments
                var diagnostic = Diagnostic.Create(
                    Rule,
                    attribute.Location,
                    attributeClass.Name);
                context.ReportDiagnostic(diagnostic);
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
