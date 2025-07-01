using Microsoft.CodeAnalysis;
using System.Text;
namespace EasyValidate.Handlers
{
    /// <summary>
    /// Generates individual private validation methods for each property.
    /// </summary>
    internal class MemberValidationMethodHandler(Compilation compilation) : ValidationHandlerBase
    {
        private readonly ValidationAttributeProcessorHandler _processor = new(compilation);

        public override void Handle(HandlerParams @params)
        {
            // Process members
            foreach (var member in @params.Members)
            {
                GeneratePropertyValidationMethod(@params.StringBuilder, member);
            }

            base.Handle(@params);
        }

        /// <summary>
        /// Generates a private validation method for a specific property.
        /// </summary>
        private void GeneratePropertyValidationMethod(StringBuilder sb, MemberInfo member)
        {
            var methodName = $"Validate{member.Name}";

            sb.AppendLine($"        private void {methodName}(ValidationResult result)");
            sb.AppendLine("        {");

            // Process all validation logic for this property
            _processor.ProcessPropertyValidation(sb, member);

            sb.AppendLine("        }");
            sb.AppendLine();
        }
    }
}
