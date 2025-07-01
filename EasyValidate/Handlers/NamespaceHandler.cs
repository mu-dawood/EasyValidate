namespace EasyValidate.Handlers
{
    internal class NamespaceHandler : ValidationHandlerBase
    {
        public override void Handle(HandlerParams @params)
        {
            var namespaceName = @params.ClassSymbol.ContainingNamespace.IsGlobalNamespace
                ? string.Empty
                : @params.ClassSymbol.ContainingNamespace.ToDisplayString();

            if (!string.IsNullOrEmpty(namespaceName))
            {
                @params.StringBuilder.AppendLine($"namespace {namespaceName}");
                @params.StringBuilder.AppendLine("{");
            }

            base.Handle(@params);

            if (!string.IsNullOrEmpty(namespaceName))
            {
                @params.StringBuilder.AppendLine("}");
            }
        }
    }
}
