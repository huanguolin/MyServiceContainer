using System;
using System.Collections.Generic;

namespace MyServiceContainer
{
    public class ServiceDescription : IServiceDescription
    {
        internal ServiceDescription _next = null;

        public ServiceDescription(
            Type serviceType,
            ServiceLifetime lifetime,
            Func<IServiceProvider, Type[], object> factory)
        {
            ServiceType = serviceType;
            Lifetime = lifetime;
            Factory = factory;
        }

        public Type ServiceType { get; set; }
        public ServiceLifetime Lifetime { get; set; }
        public Func<IServiceProvider, Type[], object> Factory { get; set; }

        internal IEnumerable<IServiceDescription> AsEnumerable()
        {
            var list = new List<IServiceDescription>();
            ServiceDescription current = this;
            while (current != null)
            {
                list.Add(current);
                current = current._next;
            }
            return list;
        }
    }
}