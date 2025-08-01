using System;

namespace EasyValidate.Core.Abstraction
{
    public struct DefaultServiceProvider : IServiceProvider
    {
        Func<Type, object?>? _action;
        public DefaultServiceProvider()
        {
        }
        public DefaultServiceProvider(IFormatter formatter)
        {
            _action = (type) => type == typeof(IFormatter) ? formatter : null;
        }
        public object? GetService(Type serviceType)
        {
            return _action?.Invoke(serviceType) ?? null;
        }
    }


}
