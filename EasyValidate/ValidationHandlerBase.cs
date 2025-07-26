using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Text;

namespace EasyValidate
{
    internal interface IValidationHandler
    {
        IValidationHandler WithNext(IValidationHandler handler);
        // public List<string> AwaitbleMembers { get; } = [];
        (StringBuilder sb, Dictionary<string, List<string>> awaitableMembers) Next(HandlerParams @params);
    }

    internal abstract class ValidationHandlerBase : IValidationHandler
    {
        private IValidationHandler? _nextHandler;

        public IValidationHandler WithNext(IValidationHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual (StringBuilder sb, Dictionary<string, List<string>> awaitableMembers) Next(HandlerParams @params)
        {
            if (_nextHandler == null)
            {
                return (new(), []);
            }
            return _nextHandler.Next(@params);
        }
    }
}

internal class HandlerParams(List<ValidationTarget> targets, SourceProductionContext context, INamedTypeSymbol classSymbol)
{
    public List<ValidationTarget> Targets { get; } = targets;
    public SourceProductionContext Context { get; } = context;
    public INamedTypeSymbol ClassSymbol { get; } = classSymbol;

}