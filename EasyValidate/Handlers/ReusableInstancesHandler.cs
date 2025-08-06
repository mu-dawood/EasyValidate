using EasyValidate.Types;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyValidate.Handlers
{
    internal class ReusableInstancesHandler : ValidationHandlerBase
    {
        public override (StringBuilder, HandlerParams) Next(HandlerParams @params)
        {
            var (nextsp, p) = base.Next(@params);
            var sb = new StringBuilder();
            var allMembers = @params.Target.Members.Select((x) => new
            {
                Member = x,
                IsStatic = x.Type.IsStatic || @params.Target.Symbol.IsStatic,
            })
            .Union(@params.Target.Methods.SelectMany((x) => x.Parmters.Select((p) => new
            {
                Member = p,
                x.Symbol.IsStatic,
            })));

            var instances = allMembers
                  .SelectMany((x) => x.Member.Attributes.Select((attr, index) => new
                  {
                      Info = attr,
                      x.IsStatic,
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
                var serviceProvider = instance.Info.NeedServiceProvider() ?
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
