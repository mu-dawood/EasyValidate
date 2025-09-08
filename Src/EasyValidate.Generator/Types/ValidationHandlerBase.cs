using System.Text;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Generator.Types;


internal abstract class ValidationHandlerBase
{
    private ValidationHandlerBase? _nextHandler;

    internal virtual ValidationHandlerBase WithNext(ValidationHandlerBase handler)
    {
        _nextHandler = handler;
        return handler;
    }

    internal virtual (StringBuilder sb, HandlerParams @params) Next(HandlerParams @params)
    {
        if (_nextHandler == null)
        {
            return (new(), @params);
        }
        return _nextHandler.Next(@params);
    }
}



internal class HandlerParams(ValidationTarget target, SourceProductionContext context, INamedTypeSymbol typeSymbol)
{
    internal ValidationTarget Target { get; } = target;
    internal SourceProductionContext Context { get; } = context;
    internal INamedTypeSymbol TypeSymbol { get; } = typeSymbol;
    internal HandlerParams WithTarget(ValidationTarget target)
    {
        return new HandlerParams(target, Context, typeSymbol);
    }

}