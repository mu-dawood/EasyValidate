using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Handlers
{
    internal class ValidateMethodOverlodsHandler : ValidationHandlerBase
    {
        public override (StringBuilder sb, Dictionary<string, List<string>> awaitableMembers) Next(HandlerParams @params)
        {
            var (nextsp, awaitableMembers) = base.Next(@params);
            var sb = new StringBuilder();
            foreach (var target in @params.Targets)
            {
                if (target.TargetType == TargetType.CurretClass)
                {
                    var returnType = "IValidationResult";
                    var methodName = "Validate";
                    if (awaitableMembers.TryGetValue(target.Symbol.Name, out var awaitableMembersList) && awaitableMembersList.Any())
                    {
                        returnType = "ValueTask<IValidationResult>";
                        methodName = "ValidateAsync";
                    }

                    // Generate Validate method with only IFormatter parameter
                    sb.AppendLine($"        public {returnType} {methodName}(IFormatter formatter)");
                    sb.AppendLine("        {");
                    sb.AppendLine("            return Validate(new DefaultServiceProvider(formatter));");
                    sb.AppendLine("        }");
                    sb.AppendLine();

                    // Generate parameterless Validate method
                    sb.AppendLine($"        public {returnType} {methodName}()");
                    sb.AppendLine("        {");
                    sb.AppendLine("            return Validate(new DefaultServiceProvider());");
                    sb.AppendLine("        }");
                }
                else if (target.TargetType == TargetType.Method)
                {
                    var methodSymbol = target.Symbol as IMethodSymbol;

                }
            }
            sb.Append(nextsp);
            return (sb, awaitableMembers);
        }
    }
}
