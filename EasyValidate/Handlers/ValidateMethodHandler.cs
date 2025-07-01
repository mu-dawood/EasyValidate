using Microsoft.CodeAnalysis;
using System.Text;
using EasyValidate.Handlers.Validation;

namespace EasyValidate.Handlers
{
    internal class ValidateMethodHandler(Compilation compilation) : ValidationHandlerBase
    {
        private readonly ValidationAttributeProcessorHandler _processor = new(compilation);

        public override void Handle(HandlerParams @params)
        {
            var sb = @params.StringBuilder;

            sb.AppendLine("        public IValidationResult Validate(IFormatter formatter, IConfigureValidator configureValidator, params string[] parentPath)");
            sb.AppendLine("        {");
            sb.AppendLine("            var result = new ValidationResult(this, parentPath, formatter, configureValidator);");
                        
            // Process members and generate validation methods
            foreach (var member in @params.Members)
            {
                if (_processor.ShouldGenerateValidationMethod(member))
                {
                    var methodName = $"Validate{member.Name}";
                    sb.AppendLine($"            {methodName}(result);");
                }
            }

            sb.AppendLine("            return result;");
            sb.AppendLine("        }");
            sb.AppendLine();
            base.Handle(@params);
        }
    }
}
