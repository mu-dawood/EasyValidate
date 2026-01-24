using System.Linq;
using System.Text;
using EasyValidate.Generator;
using EasyValidate.Generator.Helpers;
using EasyValidate.Generator.Types;

namespace EasyValidate.Handlers.Constructors
{
    /// <summary>
    /// Generates static factory methods that validate constructor parameters before creating instances.
    /// </summary>
    internal class ConstructorsRootValidatedHandler : ValidationHandlerBase
    {
        internal override (StringBuilder, HandlerParams) Next(HandlerParams @params)
        {
            var (nextsp, p) = base.Next(@params);
            var sb = new StringBuilder();

            foreach (var constructor in @params.Target.Constructors)
            {
                var className = @params.Target.Symbol.Name;
                
                // Use all parameters for method signature
                var parameters = constructor.Parameters.Select(p => $"{p.Type.SimplifiedTypeName()} {p.Name}").ToList();
                parameters.Add("ValidationConfig? config = null");
                var parametersString = string.Join(", ", parameters);
                
                // Pass all parameters to the constructor
                var passedParameters = constructor.Parameters.Select(p => $"{p.Name}: {p.Name}").ToList();
                
                var resultType = $"IValidationResult<{className}>";
                var methodName = constructor.MethodName;
                var accessModifier = constructor.AccessModifier;
                
                // Generate async or sync method based on awaitable members
                if (constructor.AwaitableMembers.Any())
                    sb.AppendLine($"        {accessModifier} static async ValueTask<{resultType}> {methodName}({parametersString})");
                else
                    sb.AppendLine($"        {accessModifier} static {resultType} {methodName}({parametersString})");
                
                sb.AppendLine("        {");
                sb.AppendLine("            var result = ValidationResult.Create();");

                // Only validate parameters that need validation
                foreach (var member in constructor.ValidatedParameters)
                {
                    var asyncItem = constructor.AwaitableMembers.Contains(member.Name);
                    var validationMethodName = $"Validate@{member.Name}@for@{methodName}".ToPascalCase();
                    if (asyncItem)
                        sb.AppendLine($"            await result.AddPropertyResultAsync({validationMethodName}({member.Name}, config));");
                    else
                        sb.AppendLine($"            result.AddPropertyResult({validationMethodName}({member.Name}, config));");
                }
                
                sb.AppendLine($"            return result.WithResult(new {className}({string.Join(", ", passedParameters)}));");
                sb.AppendLine("        }");
                sb.AppendLine();
            }

            sb.Append(nextsp);

            return (sb, p);
        }
    }
}
