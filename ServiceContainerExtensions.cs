using System;
using System.Collections.Generic;
using System.Linq;

namespace MyServiceContainer
{
    public static class ServiceContainerExtensions
    {
        public static TServiceType GetService<TServiceType>(this ServiceContainer container)
        {
            return (TServiceType)container.GetService(typeof(TServiceType));
        }

        public static ServiceContainer AddSingleton<TServiceType, TImplementType>(
            this ServiceContainer container)
        {
            return container.AddService(typeof(TServiceType),
                                        typeof(TImplementType),
                                        ServiceLifetime.Singleton);
        }

        public static ServiceContainer AddScope<TServiceType, TImplementType>(
            this ServiceContainer container)
        {
            return container.AddService(typeof(TServiceType),
                                        typeof(TImplementType),
                                        ServiceLifetime.Scope);
        }

        public static ServiceContainer AddTransient<TServiceType, TImplementType>(
            this ServiceContainer container)
        {
            return container.AddService(typeof(TServiceType),
                                        typeof(TImplementType),
                                        ServiceLifetime.Transient);
        }

        public static ServiceContainer AddService(
            this ServiceContainer container,
            Type serviceType,
            Type implementType,
            ServiceLifetime lifetime)
        {
            Func<ServiceContainer, Type[], object> factory = (_, arguments) => Create(implementType, arguments, container);
            return container.AddService(new ServiceDescription(serviceType, lifetime, factory));
        }

        private static object Create(
            Type type,
            Type[] genericArguments,
            ServiceContainer container)
        {
            if (genericArguments.Length > 0)
            {
                type = type.MakeGenericType(genericArguments);
            }

            // 策略： 构造参数最少
            var ctor = type.GetConstructors()
                .OrderBy(x => x.GetParameters().Length)
                .FirstOrDefault();

            if (ctor == null)
            {
                throw new InvalidOperationException("Require one or more public constructors.");
            }

            var args = ctor
                .GetParameters()
                .Select(x => container.GetService(x.ParameterType))
                .ToArray();
            return ctor.Invoke(args);
        }
    }
}