using Microsoft.CodeAnalysis;
using System.Text;

namespace EasyValidate
{
    internal class GeneratorChain
    {
        private IValidationHandler? _head;
        private IValidationHandler? _tail;

        internal GeneratorChain Add(IValidationHandler handler)
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

        internal void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sp)
        {
            _head?.Handle(classSymbol, context, sp);
        }
    }
}
