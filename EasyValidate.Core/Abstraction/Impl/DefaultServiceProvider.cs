namespace EasyValidate.Core.Abstraction
{
    using System;
    using System.Collections.Generic;

    public class DefaultServiceProvider : IServiceProvider
    {
        public DefaultServiceProvider()
        {
        }
        public DefaultServiceProvider(IFormatter formatter)
        {
            _factories[typeof(IFormatter)] = () => formatter;
        }
        private readonly Dictionary<Type, Func<object?>> _factories = [];

        public void Register<TService>(TService instance)
        {
            _factories[typeof(TService)] = () => instance;
        }

        public object? GetService(Type serviceType)
        {
            if (_factories.TryGetValue(serviceType, out var factory))
            {
                return factory();
            }

            //Support fallback creation via reflection
            if (!serviceType.IsAbstract && !serviceType.IsInterface)
            {
                try
                {
                    return Activator.CreateInstance(serviceType);
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }
    }

}
