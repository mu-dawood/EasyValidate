using System.Linq;
using System.Text;
using EasyValidate.Generator;
using EasyValidate.Generator.Types;

namespace EasyValidate.Handlers
{
    internal class RootValidateMethodHandler : ValidationHandlerBase
    {

        internal override (StringBuilder, HandlerParams) Next(HandlerParams @params)
        {
            var (nextsp, p) = base.Next(@params);
            if (p.Target.Members.Count == 0)
                return (nextsp, p);

            var sb = new StringBuilder();
            if (p.Target.AwaitableMembers.Any())
                sb.AppendLine("        public async ValueTask<IValidationResult> ValidateAsync(ValidationConfig? config=null)");
            else
                sb.AppendLine("        public IValidationResult Validate(ValidationConfig? config = null)");
            sb.AppendLine("        {");
            sb.AppendLine("            var result = ValidationResult.Create();");


            foreach (var member in @params.Target.Members)
            {
                var asyncItem = p.Target.AwaitableMembers.Contains(member.Name);
                var methodName = $"Validate@{member.Name}".ToPascalCase();
                if (asyncItem)
                    sb.AppendLine($"            await result.AddPropertyResultAsync({methodName}(config));");
                else
                    sb.AppendLine($"            result.AddPropertyResult({methodName}(config));");

            }
            sb.AppendLine("            return result;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.Append(nextsp);
            return (sb, p);
        }
    }
}
