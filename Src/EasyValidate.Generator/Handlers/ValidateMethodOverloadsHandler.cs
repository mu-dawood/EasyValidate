using System.Linq;
using System.Text;
using EasyValidate.Generator.Types;

namespace EasyValidate.Handlers
{
    internal class ValidateMethodOverloadsHandler : ValidationHandlerBase
    {
        internal override (StringBuilder, HandlerParams) Next(HandlerParams @params)
        {
            var (nextsp, p) = base.Next(@params);
            if (p.Target.Members.Count == 0)
                return (nextsp, p);
                
            var sb = new StringBuilder();

            var returnType = "IValidationResult";
            var methodName = "Validate";
            if (p.Target.AwaitableMembers.Any())
            {
                returnType = "ValueTask<IValidationResult>";
                methodName = "ValidateAsync";
            }

            sb.AppendLine($"        public {returnType} {methodName}(Action<ValidationConfig> configure)");
            sb.AppendLine("        {");
            sb.AppendLine("            var config = new ValidationConfig();");
            sb.AppendLine("            configure.Invoke(config);");
            sb.AppendLine($"            return {methodName}(config);");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.Append(nextsp);
            return (sb, p);
        }
    }
}
