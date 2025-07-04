using Microsoft.CodeAnalysis;
using System.Text;

namespace EasyValidate.Handlers
{
    internal class ValidateMethodOverlodsHandler : ValidationHandlerBase
    {
        public override void Handle(HandlerParams @params)
        {
            var sb = @params.StringBuilder;
            // Generate Validate method with only IFormatter parameter
            sb.AppendLine("        public IValidationResult Validate(IFormatter formatter, params string[] parentPath)");
            sb.AppendLine("        {");
            sb.AppendLine("            return Validate(formatter, null, parentPath);");
            sb.AppendLine("        }");
            sb.AppendLine();

            // Generate Validate method with only IConfigureValidator parameter
            sb.AppendLine("        public IValidationResult Validate(IConfigureValidator configureValidator, params string[] parentPath)");
            sb.AppendLine("        {");
            sb.AppendLine("            return Validate(null, configureValidator, parentPath);");
            sb.AppendLine("        }");
            sb.AppendLine();

            // Generate parameterless Validate method
            sb.AppendLine("        public IValidationResult Validate(params string[] parentPath)");
            sb.AppendLine("        {");
            sb.AppendLine("            return Validate(null, null, parentPath);");
            sb.AppendLine("        }");

            base.Handle(@params);
        }
    }
}
