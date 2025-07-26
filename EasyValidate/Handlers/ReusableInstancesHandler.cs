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
            var instances = @params.Targets
                  .SelectMany((x) => x.Members)
                  .SelectMany((x) => x.Attributes.Select((attr, index) => new
                  {
                      Info = attr,
                      x.Type.IsStatic,
                  }))
                  .GroupBy(x => x.Info.InstanceName)
                  .Select(g => g.OrderByDescending((o) => o.IsStatic).FirstOrDefault())
                  .Where(x => x != null)
                  .ToList();


            // Emit reusable static attribute instances before Validate methods

            foreach (var instance in instances)
            {
                if (instance.IsStatic)
                    sb.AppendLine($"        private static readonly {instance.Info.Attribute.AttributeClass?.Name} {instance.Info.InstanceName} = {instance.Info.InstanceDeclration};");
                else
                    sb.AppendLine($"        private readonly {instance.Info.Attribute.AttributeClass?.Name} {instance.Info.InstanceName} = {instance.Info.InstanceDeclration};");
            }
            sb.AppendLine();
            sb.Append(nextsp);
            return (sb, awaitableMembers);
        }
    }
}
