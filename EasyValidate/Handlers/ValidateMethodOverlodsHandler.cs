using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyValidate.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Handlers
{
    internal class ValidateMethodOverlodsHandler : ValidationHandlerBase
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

            // Generate Validate method with only IFormatter parameter
            sb.AppendLine($"        public {returnType} {methodName}(IFormatter formatter)");
            sb.AppendLine("        {");
            sb.AppendLine($"            return {methodName}(new DefaultServiceProvider(formatter));");
            sb.AppendLine("        }");
            sb.AppendLine();

            // Generate parameterless Validate method
            sb.AppendLine($"        public {returnType} {methodName}()");
            sb.AppendLine("        {");
            sb.AppendLine($"            return {methodName}(new DefaultServiceProvider());");
            sb.AppendLine("        }");


            sb.Append(nextsp);
            return (sb, awaitableMembers);
        }
    }
}
