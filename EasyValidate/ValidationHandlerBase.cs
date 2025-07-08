using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Text;

namespace EasyValidate
{
    internal interface IValidationHandler
    {
        IValidationHandler WithNext(IValidationHandler handler);
        void Handle(HandlerParams @params);
    }

    internal abstract class ValidationHandlerBase : IValidationHandler
    {
        private IValidationHandler? _nextHandler;

        public IValidationHandler WithNext(IValidationHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual void Handle(HandlerParams @params)
        {
            _nextHandler?.Handle(@params);
        }
    }
}

public class HandlerParams(List<MemberInfo> members, SourceProductionContext context, StringBuilder stringBuilder, INamedTypeSymbol classSymbol)
{
    public List<MemberInfo> Members { get; } = members;
    public SourceProductionContext Context { get; } = context;
    public StringBuilder StringBuilder { get; } = stringBuilder;
    public INamedTypeSymbol ClassSymbol { get; } = classSymbol;
}