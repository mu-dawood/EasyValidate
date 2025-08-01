using EasyValidate.Types;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyValidate.Handlers
{
    internal class ReusableInstancesHandler : ValidationHandlerBase
    {
        public override (StringBuilder sb, Dictionary<string, List<string>> awaitableMembers) Next(HandlerParams @params)
        {
            var (nextsp, awaitableMembers) = base.Next(@params);
            var sb = new StringBuilder();
            var instances = @params.Target.Members
                  .SelectMany((x) => x.Attributes.Select((attr, index) => new
                  {
                      Info = attr,
                      x.Type.IsStatic,
                  }))
                  .GroupBy(x => x.Info.InstanceVariable)
                  .Select(g => g.OrderByDescending((o) => o.IsStatic).FirstOrDefault())
                  .Where(x => x != null)
                  .ToList();


            // Emit reusable static attribute instances before Validate methods

            foreach (var instance in instances)
            {
                var attributeClassName = instance.Info.Attribute.AttributeClass?.Name;
                var instanceVariable = instance.Info.InstanceVariable;
                var instanceDeclration = instance.Info.InstanceDeclration;
                var instanceMethod = instance.Info.InstanceMethod;
                var staticModifier = instance.IsStatic ? "static " : string.Empty;

                sb.AppendLine($"        private {staticModifier}{attributeClassName}? {instanceVariable};");
                sb.AppendLine($"        private {staticModifier}{attributeClassName} {instanceMethod} (IServiceProvider serviceProvider) => {instanceVariable} ??= {instanceDeclration};");

            }
            sb.AppendLine();
            sb.Append(nextsp);
            return (sb, awaitableMembers);
        }
    }
}
