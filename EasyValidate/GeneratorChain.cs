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

        internal void Handle(HandlerParams @params)
        {
            _head?.Handle(@params);
        }
    }
}
