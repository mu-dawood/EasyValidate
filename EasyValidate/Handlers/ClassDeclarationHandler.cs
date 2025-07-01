using Microsoft.CodeAnalysis;
using System.Text;

namespace EasyValidate.Handlers
{
    internal class ClassDeclarationHandler : ValidationHandlerBase
    {
        public override void  Handle(HandlerParams @params)
        {
            @params.StringBuilder.AppendLine($"    public partial class {@params.ClassSymbol.Name} : IValidate");
            @params.StringBuilder.AppendLine("    {");

            base.Handle(@params);

            @params.StringBuilder.AppendLine("    }");
        }
    }
}
