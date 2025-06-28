using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;

namespace EasyValidate.Handlers.Validation
{
    /// <summary>
    /// Handles nested validation for collections and objects with validation attributes.
    /// </summary>
    internal class NestedValidationHandler
    {
        /// <summary>
        /// Processes nested validation for properties that are collections or objects with validation attributes.
        /// </summary>
        public void ProcessNestedValidation(StringBuilder sb, IPropertySymbol member)
        {
            ProcessNestedValidationCore(sb, member.Name, member.Type);
        }

        /// <summary>
        /// Processes nested validation for fields that are collections or objects with validation attributes.
        /// </summary>
        public void ProcessNestedValidationForField(StringBuilder sb, IFieldSymbol member)
        {
            ProcessNestedValidationCore(sb, member.Name, member.Type);
        }

        /// <summary>
        /// Core logic for processing nested validation for any member type.
        /// </summary>
        private void ProcessNestedValidationCore(StringBuilder sb, string memberName, ITypeSymbol memberType)
        {

            // Check if it's a collection type
            var isCollection = IsCollection(memberType);

            // Handle nested validation - check if this member itself is a collection first
            if (isCollection)
            {
                // This is a collection (array, list, etc.) - use collection validation
                sb.AppendLine($"            if ({memberName} != null) result.MergeWith(nameof({memberName}), {memberName});");
            }

            // Check if the member's type has any properties with validation attributes
            var hasPropertyValidationAttributes = memberType.GetMembers()
                .OfType<IPropertySymbol>()
                .Any(subMember => subMember.GetAttributes().Any(attr => attr.AttributeClass.IsValidationAttribute(out _)));

            // Check if the member's type has any fields with validation attributes
            var hasFieldValidationAttributes = memberType.GetMembers()
                .OfType<IFieldSymbol>()
                .Any(subMember => subMember.GetAttributes().Any(attr => attr.AttributeClass.IsValidationAttribute(out _)));

            var hasValidationAttributes = hasPropertyValidationAttributes || hasFieldValidationAttributes;


            if (hasValidationAttributes)
            {
                // This is a single object with validation attributes
                sb.AppendLine($"            if ({memberName} != null) result.MergeWith(nameof({memberName}), {memberName});");
            }
        }

        /// <summary>
        /// Determines if a type is a collection type.
        /// </summary>
        private bool IsCollection(ITypeSymbol type)
        {
            // Exclude string type (even though it implements IEnumerable<char>)
            if (type.SpecialType == SpecialType.System_String)
                return false;

            // Check for arrays first
            if (type is IArrayTypeSymbol)
                return true;

            // Check if it's a collection type (implements IEnumerable)
            if (type is INamedTypeSymbol namedType)
            {
                // Check if the collection type implements IEnumerable (generic or non-generic)
                bool isEnumerable = namedType.AllInterfaces.Any(i =>
                {
                    var interfaceName = i.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                    return interfaceName.StartsWith("global::System.Collections.Generic.IEnumerable") ||
                           interfaceName == "global::System.Collections.IEnumerable";
                });

                return isEnumerable;
            }

            return false;
        }
    }
}
