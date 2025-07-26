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

        internal (StringBuilder sb, Dictionary<string, List<string>> awaitableMembers) Handle(HandlerParams @params)
        {
            return _head.Next(@params);
        }
    }
}
