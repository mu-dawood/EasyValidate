using System.Linq;

namespace EasyValidate.Handlers
{
    internal class UsingImportsHandler : ValidationHandlerBase
    {
        public override void Handle(HandlerParams @params)
        {
            @params.StringBuilder.AppendLine("using global::System;");
            @params.StringBuilder.AppendLine("using global::System.Collections.Generic;");
            @params.StringBuilder.AppendLine("using global::EasyValidate.Core.Abstraction;");
            @params.StringBuilder.AppendLine("using global::EasyValidate.Core.Attributes;");

            base.Handle(@params);
        }
    }
}
