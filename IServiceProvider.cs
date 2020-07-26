using System;

namespace MyServiceContainer
{
    public interface IServiceProvider
    {
        void RegistryService(IServiceDescription serviceDescription);
        object GetService(Type serviceType);
    }
}