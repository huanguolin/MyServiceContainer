using System;
using System.Collections.Generic;

namespace MyServiceContainer
{
    public class ServiceContainer : IServiceContainer, IServiceProvider, IDisposable
    {
        private readonly Dictionary<Key, ServiceDescription> _serviceDescriptionDict;
        private readonly Dictionary<Key, object> _serviceInstanceDict;
        private readonly List<IDisposable> _disposableDict;
        internal readonly ServiceContainer _root;

        public ServiceContainer()
        {
        }

        // public ServiceContainer()
        // {
        // }

        public IServiceProvider CreateScopeContainer(IServiceContainer root)
        {
            throw new System.NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public void RegistryService(IServiceDescription serviceDescription)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}