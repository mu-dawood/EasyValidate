using Microsoft.CodeAnalysis;
using System.Text;

namespace EasyValidate.Handlers
{
    internal class UsingImportsHandler : ValidationHandlerBase
    {
        public override void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sb)
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using EasyValidate.Core.Abstraction;");
            sb.AppendLine("using EasyValidate.Core.Attributes;");

            base.Handle(classSymbol, context, sb);
        }
    }
}
