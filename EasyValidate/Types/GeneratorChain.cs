using System.Text;

namespace EasyValidate.Generator.Types
{
    internal class GeneratorChain(ValidationHandlerBase head)
    {
        private readonly ValidationHandlerBase _head = head;
        private ValidationHandlerBase _tail = head;

        internal GeneratorChain Add(ValidationHandlerBase handler)
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
