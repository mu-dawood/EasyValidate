using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyValidate.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Handlers
{
    internal class ValidateMethodOverloadsHandler : ValidationHandlerBase
    {
        public override (StringBuilder sb, Dictionary<string, List<string>> awaitableMembers) Next(HandlerParams @params)
        {
            var (nextsp, awaitableMembers) = base.Next(@params);
            var sb = new StringBuilder();

            var returnType = "IValidationResult";
            var methodName = "Validate";
            if (awaitableMembers.TryGetValue(@params.Target.Symbol.Name, out var awaitableMembersList) && awaitableMembersList.Any())
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
            return (sb, awaitableMembers);
        }
    }
}
