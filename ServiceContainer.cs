using System;
using System.Collections.Generic;

namespace MyServiceContainer
{
    public class ServiceContainer : IServiceContainer, IServiceProvider, IDisposable
    {
        private readonly Dictionary<Key, ServiceDescription> _serviceDescriptionDict;
        private readonly Dictionary<Key, object> _serviceInstanceDict;
        private readonly List<IDisposable> _disposables;
        internal readonly ServiceContainer _root;

        public ServiceContainer()
        {
            _serviceDescriptionDict = new Dictionary<Key, ServiceDescription>();
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
            foreach (var item in _disposables)
            {
                item.Dispose();
            }
        }
    }
}