using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyValidate.Helpers;
using EasyValidate.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Handlers
{
    internal class MethodsRootValidatedHandler : ValidationHandlerBase
    {

        public override (StringBuilder, HandlerParams) Next(HandlerParams @params)
        {
            var (nextsp, p) = base.Next(@params);
            var sb = new StringBuilder();

            foreach (var method in @params.Target.Methods)
            {
                var parmters = method.Parmters.Select(p => $"{p.Type.SimplifiedTypeName()} {p.Name}").ToList();
                parmters.Add("ValidationConfig? config = null");
                var parmtersString = string.Join(", ", parmters);
                var rootMethodName = method.Symbol.Name;
                var returnType = method.Symbol.ReturnType.SimplifiedTypeName();
                bool isVoid = returnType == "void";
                var resultType = isVoid ? "IValidationResult" : $"IValidationResult<{returnType}>";
                var instanceType = isVoid ? "IValidationResult" : $"IValidationResult<{returnType}>";
                var passedPramters = method.Parmters.Select(p => $"{p.Name}: {p.Name}").ToList();
                var staticMethod = method.Symbol.IsStatic ? "static " : string.Empty;
                if (method.AwaitableMembers.Any())
                    sb.AppendLine($"        public {staticMethod}async ValueTask<{resultType}> {rootMethodName}({parmtersString})");
                else
                    sb.AppendLine($"        public {staticMethod}{resultType} {rootMethodName}({parmtersString})");
                sb.AppendLine("        {");
                sb.AppendLine("            var result = ValidationResult.Create();");

                foreach (var member in method.Parmters)
                {
                    var asyncItem = method.AwaitableMembers.Contains(member.Name);
                    var methodName = $"Validate@{member.Name}@for@{method.Symbol.Name}".ToPascalCase();
                    if (asyncItem)
                        sb.AppendLine($"            await result.AddPropertyResultAsync({methodName}({member.Name}, config));");
                    else
                        sb.AppendLine($"            result.AddPropertyResult({methodName}({member.Name}, config));");

                }
                if (isVoid)
                    sb.AppendLine("            return result;");
                else
                    sb.AppendLine($"            return result.WithResult({rootMethodName}({string.Join(", ", passedPramters)}));");
                sb.AppendLine("        }");
                sb.AppendLine();
            }

            sb.Append(nextsp);

            return (sb, p);
        }
    }
}
