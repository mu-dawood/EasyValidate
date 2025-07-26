using System.Collections.Generic;
using System.Text;

namespace EasyValidate
{
    internal class GeneratorChain(IValidationHandler head)
    {
        private IValidationHandler _head = head;
        private IValidationHandler? _tail;

        internal GeneratorChain Add(IValidationHandler handler)
        {
            if (_tail != null)
            {
                _tail.WithNext(handler);
                _tail = handler;
            }

            return this;
        }

        internal (StringBuilder sb, Dictionary<string, List<string>> awaitableMembers) Handle(HandlerParams @params)
        {
            return _head.Next(@params);
        }
    }
}
