using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyValidate.Generator.Types;

namespace EasyValidate.Handlers
{
    internal class ReusableInstancesHandler : ValidationHandlerBase
    {
        internal override (StringBuilder, HandlerParams) Next(HandlerParams @params)
        {
            var (nextsp, p) = base.Next(@params);
            var sb = new StringBuilder();


            List<(AttributeInfo info, bool IsStatic)> attributes = [];

            foreach (var member in p.Target.Members)
            {
                attributes.AddRange(member.Attributes.Values.SelectMany((attr) => attr.Select((x) => (x, member.Type.IsStatic || p.Target.Symbol.IsStatic))));
            }
            foreach (var method in p.Target.Methods)
            {
                attributes.AddRange(method.Parmters.SelectMany((x) => x.Attributes.Values.SelectMany((attr) => attr.Select((y) => (y, method.Symbol.IsStatic)))));
            }

            var instances = attributes
                  .GroupBy(x => x.info.InstanceVariable)
                  .Select(g => g.OrderByDescending((o) => o.IsStatic).FirstOrDefault())
                  .ToList();


            // Emit reusable static attribute instances before Validate methods

            foreach (var instance in instances)
            {
                var attributeClassName = instance.info.Attribute.AttributeClass?.Name;
                var instanceVariable = instance.info.InstanceVariable;
                var instanceDeclration = instance.info.InstanceDeclration;
                var instanceMethod = instance.info.InstanceMethod;
                var staticModifier = instance.IsStatic ? "static " : string.Empty;
                var serviceProvider = instance.info.NeedServiceProvider() ?
                    "ValidationConfig? config" : string.Empty;
                sb.AppendLine($"        private {staticModifier}{attributeClassName}? {instanceVariable};");
                sb.AppendLine($"        private {staticModifier}{attributeClassName} {instanceMethod} ({serviceProvider}) => {instanceVariable} ??= {instanceDeclration};");

            }
            sb.AppendLine();
            sb.Append(nextsp);
            return (sb, p);
        }
    }
}
