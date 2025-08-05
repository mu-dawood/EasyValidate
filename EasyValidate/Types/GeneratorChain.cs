using System.Collections.Generic;
using System.Text;

namespace EasyValidate.Types
{
    internal class GeneratorChain(IValidationHandler head)
    {
        private readonly IValidationHandler _head = head;
        private IValidationHandler _tail = head;

        internal GeneratorChain Add(IValidationHandler handler)
        {
            _tail = _tail.WithNext(handler);
            return this;
        }

        internal StringBuilder Handle(HandlerParams @params)
        {
            var (sb, _) = _head.Next(@params);
            return sb;
        }
    }
}
