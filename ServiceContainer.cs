using System;
using System.Linq;
using System.Collections.Generic;

namespace MyServiceContainer
{
    public class ServiceContainer : IDisposable
    {
        private readonly Dictionary<Type, ServiceDescription> _serviceDescriptionDict;
        private readonly Dictionary<Key, object> _serviceInstanceDict;
        private readonly List<IDisposable> _disposables;
        internal readonly ServiceContainer _root;

        public ServiceContainer()
        {
            _root = this;
            _serviceDescriptionDict = new Dictionary<Type, ServiceDescription>();
            _serviceInstanceDict = new Dictionary<Key, object>();
            _disposables = new List<IDisposable>();
        }

        internal ServiceContainer(ServiceContainer parent)
        {
            _root = parent._root;
            _serviceDescriptionDict = _root._serviceDescriptionDict;
            _serviceInstanceDict = new Dictionary<Key, object>();
            _disposables = new List<IDisposable>();
        }

        public ServiceContainer CreateScopeContainer()
        {
            return new ServiceContainer(this);
        }

        public object GetService(Type serviceType)
        {
            // 不考虑 serviceDescription.serviceType 是 ServiceContainer 的情况

            // IEnumerable<T>
            if (serviceType.IsGenericType
                && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var realServiceType = serviceType.GetGenericArguments()[0];

                if (!_serviceDescriptionDict.TryGetValue(realServiceType, out var headerDescription))
                {
                    throw new InvalidOperationException("You should add service first!");
                }

                return headerDescription
                    .AsEnumerable()
                    .Select(x => GetService(x, Type.EmptyTypes))
                    .ToList();
            }

            if (!_serviceDescriptionDict.TryGetValue(serviceType, out var serviceDescription))
            {
                throw new InvalidOperationException("You should add service first!");
            }

            return GetService(
                serviceDescription,
                serviceType.IsGenericType ? serviceType.GetGenericArguments() : Type.EmptyTypes);
        }

        public ServiceContainer AddService(ServiceDescription serviceDescription)
        {
            if (_serviceDescriptionDict.TryGetValue(serviceDescription.ServiceType, out var exists))
            {
                // 后来居上
                _serviceDescriptionDict.Add(serviceDescription.ServiceType, serviceDescription);
                serviceDescription.Next = exists;
            }
            else
            {
                _serviceDescriptionDict.Add(serviceDescription.ServiceType, serviceDescription);
            }
            return this;
        }

        public void Dispose()
        {
            // TODO 每个接口调用前都要确认是否已经 Dispose
            foreach (var item in _disposables)
            {
                item.Dispose();
            }
        }

        private object GetService(ServiceDescription serviceDescription, Type[] genericArguments)
        {
            var key = new Key(serviceDescription, genericArguments);
            Func<object> factory = () => serviceDescription.Factory(this, genericArguments);
            return serviceDescription.Lifetime switch
            {
                ServiceLifetime.Singleton => GetOrCreateServiceInstance(factory,
                                                                        key,
                                                                        _root._serviceInstanceDict,
                                                                        _root._disposables),
                ServiceLifetime.Scope => GetOrCreateServiceInstance(factory,
                                                                    key,
                                                                    _serviceInstanceDict,
                                                                    _disposables),
                _ => CreateServiceInstance(factory,
                                           key,
                                           null,
                                           _disposables),
            };
        }

        private object GetOrCreateServiceInstance(
            Func<object> factory,
            Key key,
            Dictionary<Key, object> serviceInstanceDict,
            List<IDisposable> disposables)
        {
            if (serviceInstanceDict.TryGetValue(key, out var instance))
            {
                return instance;
            }
            else
            {
                return CreateServiceInstance(factory,
                                             key,
                                             serviceInstanceDict,
                                             disposables);
            }
        }

        private object CreateServiceInstance(
            Func<object> factory,
            Key key,
            Dictionary<Key, object> serviceInstanceDict,
            List<IDisposable> disposables)
        {
            var instance = factory();

            if (serviceInstanceDict != null)
            {
                serviceInstanceDict.Add(key, instance);
            }

            if (instance is IDisposable disposable)
            {
                disposables.Add(disposable);
            }

            return instance;
        }
    }
}