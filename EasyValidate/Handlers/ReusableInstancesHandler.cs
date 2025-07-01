using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;

namespace EasyValidate.Handlers
{
    internal class ReusableInstancesHandler : ValidationHandlerBase
    {
        public override void Handle(HandlerParams @params)
        {
            var sb = @params.StringBuilder;
            var instances = @params.Members
                  .SelectMany((x) => x.Attributes)
                  .GroupBy(x => x.InstanceName)
                  .Select(g => g.FirstOrDefault())
                  .Where(x => x != null)
                  .ToList();


            // Emit reusable static attribute instances before Validate methods
            foreach (var instance in instances)
            {
                // Example: public static readonly NotNullAttribute NotNull = NotNullAttribute.Instance;
                sb.AppendLine($"        private static readonly {instance.Attribute.AttributeClass?.Name} {instance.InstanceName} = {instance.InstanceDeclration};");
            }
            sb.AppendLine();
            base.Handle(@params);
        }
    }
}
