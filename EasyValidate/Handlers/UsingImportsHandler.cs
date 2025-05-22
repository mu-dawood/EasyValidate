using Microsoft.CodeAnalysis;
using System.Text;

namespace EasyValidate.Handlers
{
    public class UsingImportsHandler : ValidationHandlerBase
    {
        public override void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sb)
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using EasyValidate.Abstraction;");

            base.Handle(classSymbol, context, sb);
        }
    }
}
