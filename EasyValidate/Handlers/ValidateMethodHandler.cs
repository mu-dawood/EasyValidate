using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyValidate.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Handlers
{
    internal class ValidateMethodHandler(Compilation compilation) : ValidationHandlerBase
    {
        private readonly ValidationAttributeProcessorHandler _processor = new(compilation);

        public override (StringBuilder sb, Dictionary<string, List<string>> awaitableMembers) Next(HandlerParams @params)
        {
            var currentClassTarget = @params.Targets
                .FirstOrDefault(t => t.TargetType == TargetType.CurretClass);
            if (currentClassTarget == null) return base.Next(@params);
            var (nextsp, awaitableMembers) = base.Next(@params);
            var sb = new StringBuilder();
            if (awaitableMembers.TryGetValue(currentClassTarget.Symbol.Name, out var awaitableMembersList) && awaitableMembersList.Any())
                sb.AppendLine("        public async ValueTask<IValidationResult> ValidateAsync(IServiceProvider serviceProvider)");
            else
                sb.AppendLine("        public IValidationResult Validate(IServiceProvider serviceProvider)");
            sb.AppendLine("        {");
            sb.AppendLine("            var result = new ValidationResult();");
            awaitableMembersList ??= [];
            // Process members and generate validation methods
            foreach (var member in currentClassTarget.Members)
            {
                var awitKeyWord = awaitableMembersList.Contains(member.Name) ? "await " : "";
                var methodName = $"Validate@{member.Name}".ToPascalCase();
                sb.AppendLine($"            result.AddPropertyResult({awitKeyWord}{methodName}(serviceProvider));");
            }

            sb.AppendLine("            return result;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.Append(nextsp);
            return (sb, awaitableMembers);
        }
    }
}
