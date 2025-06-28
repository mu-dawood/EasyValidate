using Microsoft.CodeAnalysis;
using System.Text;
using EasyValidate.Handlers.Validation;

namespace EasyValidate.Handlers
{
    /// <summary>
    /// Generates individual private validation methods for each property.
    /// </summary>
    internal class PropertyValidationMethodHandler(Compilation compilation) : ValidationHandlerBase
    {
        private readonly ValidationAttributeProcessorHandler _processor = new(compilation);

        public override void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sb)
        {
            // Process properties
            foreach (var member in classSymbol.GetMembers().OfType<IPropertySymbol>())
            {
                if (_processor.ShouldGenerateValidationMethod(member))
                {
                    GeneratePropertyValidationMethod(sb, member);
                }
            }

            // Process fields
            foreach (var member in classSymbol.GetMembers().OfType<IFieldSymbol>())
            {
                if (_processor.ShouldGenerateValidationMethod(member))
                {
                    GenerateFieldValidationMethod(sb, member);
                }
            }

            base.Handle(classSymbol, context, sb);
        }

        /// <summary>
        /// Generates a private validation method for a specific property.
        /// </summary>
        private void GeneratePropertyValidationMethod(StringBuilder sb, IPropertySymbol member)
        {
            var methodName = $"Validate{member.Name}";
            
            sb.AppendLine($"        private void {methodName}(ValidationResult result)");
            sb.AppendLine("        {");

            // Process all validation logic for this property
            _processor.ProcessPropertyValidation(sb, member);

            sb.AppendLine("        }");
            sb.AppendLine();
        }

        /// <summary>
        /// Generates a private validation method for a specific field.
        /// </summary>
        private void GenerateFieldValidationMethod(StringBuilder sb, IFieldSymbol member)
        {
            var methodName = $"Validate{member.Name}";
            
            sb.AppendLine($"        private void {methodName}(ValidationResult result)");
            sb.AppendLine("        {");

            // Process all validation logic for this field
            _processor.ProcessFieldValidation(sb, member);

            sb.AppendLine("        }");
            sb.AppendLine();
        }
    }
}
