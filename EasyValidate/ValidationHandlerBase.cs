using Microsoft.CodeAnalysis;
using System.Text;

namespace EasyValidate
{
    internal interface IValidationHandler
    {
        IValidationHandler WithNext(IValidationHandler handler);
        void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sp);
    }

    internal abstract class ValidationHandlerBase : IValidationHandler
    {
        private IValidationHandler? _nextHandler;

        public IValidationHandler WithNext(IValidationHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sp)
        {
            _nextHandler?.Handle(classSymbol, context, sp);
        }
    }
}
