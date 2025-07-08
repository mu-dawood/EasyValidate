using Microsoft.CodeAnalysis;

namespace EasyValidate.Handlers
{
    internal class ValidateMethodHandler(Compilation compilation) : ValidationHandlerBase
    {
        private readonly ValidationAttributeProcessorHandler _processor = new(compilation);

        public override void Handle(HandlerParams @params)
        {
            var sb = @params.StringBuilder;

            sb.AppendLine("        public IValidationResult Validate(IFormatter? formatter, IConfigureValidator? configureValidator, params string[] parentPath)");
            sb.AppendLine("        {");
            sb.AppendLine("            var result = new ValidationResult(formatter, configureValidator, parentPath);");

            // Process members and generate validation methods
            foreach (var member in @params.Members)
            {

                var methodName = $"Validate@{member.Name}".ToPascalCase();
                sb.AppendLine($"            {methodName}(result);");

            }

            sb.AppendLine("            return result;");
            sb.AppendLine("        }");
            sb.AppendLine();
            base.Handle(@params);
        }
    }
}
