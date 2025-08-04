using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyValidate.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Handlers
{
    internal class RootValidateMethodHandler : ValidationHandlerBase
    {

        public override (StringBuilder sb, Dictionary<string, List<string>> awaitableMembers) Next(HandlerParams @params)
        {
            var (nextsp, awaitableMembers) = base.Next(@params);
            var sb = new StringBuilder();
            if (awaitableMembers.TryGetValue(@params.Target.Symbol.Name, out var awaitableMembersList) && awaitableMembersList.Any())
                sb.AppendLine("        public async ValueTask<IValidationResult> ValidateAsync(ValidationConfig config)");
            else
                sb.AppendLine("        public IValidationResult Validate(ValidationConfig? config = null)");
            sb.AppendLine("        {");
            sb.AppendLine("            var result = new ValidationResult();");

            awaitableMembersList ??= [];
            foreach (var member in @params.Target.Members)
            {
                var asyncItem = awaitableMembersList.Contains(member.Name);
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
            return (sb, awaitableMembers);
        }
    }
}
