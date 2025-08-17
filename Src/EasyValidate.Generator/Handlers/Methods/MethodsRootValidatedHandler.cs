using System.Linq;
using System.Text;
using EasyValidate.Generator;
using EasyValidate.Generator.Helpers;
using EasyValidate.Generator.Types;

namespace EasyValidate.Handlers.Methods
{
    internal class MethodsRootValidatedHandler : ValidationHandlerBase
    {

        internal override (StringBuilder, HandlerParams) Next(HandlerParams @params)
        {
            var (nextsp, p) = base.Next(@params);
            var sb = new StringBuilder();

            foreach (var method in @params.Target.Methods)
            {
                var parmters = method.Parmters.Select(p => $"{p.Type.SimplifiedTypeName()} {p.Name}").ToList();
                parmters.Add("ValidationConfig? config = null");
                var parmtersString = string.Join(", ", parmters);
                var rootMethodName = method.Symbol.Name;
                var (isAsyncMethod, args) = method.Symbol.IsAsyncMethod();
                var returnType = isAsyncMethod ? string.Join(", ", args.Select((x) => x.SimplifiedTypeName())) : method.Symbol.ReturnType.SimplifiedTypeName();
                bool isVoid = returnType == "void" || (isAsyncMethod && args.Length == 0);
                var resultType = isVoid ? "IValidationResult" : $"IValidationResult<{returnType}>";
                var instanceType = isVoid ? "IValidationResult" : $"IValidationResult<{returnType}>";
                var passedPramters = method.Parmters.Select(p => $"{p.Name}: {p.Name}").ToList();
                var staticMethod = method.Symbol.IsStatic ? "static " : string.Empty;
                if (isAsyncMethod || method.AwaitableMembers.Any())
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
                var asyncPrefix = isAsyncMethod ? "await " : string.Empty;
                if (isVoid)
                {
                    sb.AppendLine($"            {asyncPrefix}{rootMethodName}({string.Join(", ", passedPramters)});");
                    sb.AppendLine("            return result;");
                }
                else
                    sb.AppendLine($"            return result.WithResult({asyncPrefix}{rootMethodName}({string.Join(", ", passedPramters)}));");
                sb.AppendLine("        }");
                sb.AppendLine();
            }

            sb.Append(nextsp);

            return (sb, p);
        }
    }
}
