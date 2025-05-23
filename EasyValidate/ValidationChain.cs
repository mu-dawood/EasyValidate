using Microsoft.CodeAnalysis;
using System.Text;

namespace EasyValidate
{
    public class ValidationChain
    {
        private IValidationHandler? _head;
        private IValidationHandler? _tail;

        public ValidationChain Add(IValidationHandler handler)
        {
            if (_head == null)
            {
                _head = handler;
                _tail = handler;
            }
            else if (_tail != null)
            {
                _tail.WithNext(handler);
                _tail = handler;
            }

            return this;
        }

        public void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sp)
        {
            _head?.Handle(classSymbol, context, sp);
        }
    }
}
