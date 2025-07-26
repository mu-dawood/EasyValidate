using System.Collections.Generic;
using System.Text;

namespace EasyValidate.Handlers
{
    internal class NamespaceHandler : ValidationHandlerBase
    {
        public override (StringBuilder sb, Dictionary<string, List<string>> awaitableMembers) Next(HandlerParams @params)
        {
            var namespaceName = @params.ClassSymbol.ContainingNamespace.IsGlobalNamespace
                ? string.Empty
                : @params.ClassSymbol.ContainingNamespace.ToDisplayString();

            var (nextsp, awaitableMembers) = base.Next(@params);
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(namespaceName))
            {
                sb.AppendLine($"namespace {namespaceName}");
                sb.AppendLine("{");
            }

            sb.Append(nextsp);

            if (!string.IsNullOrEmpty(namespaceName))
            {
                sb.AppendLine("}");
            }
            return (sb, awaitableMembers);
        }
    }
}
