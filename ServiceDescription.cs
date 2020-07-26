using System;
using System.Collections.Generic;

namespace MyServiceContainer
{
    public class ServiceDescription
    {
        public ServiceDescription(
            Type serviceType,
            ServiceLifetime lifetime,
            Func<ServiceContainer, Type[], object> factory)
        {
            ServiceType = serviceType;
            Lifetime = lifetime;
            Factory = factory;
        }

        public Type ServiceType { get; set; }
        public ServiceLifetime Lifetime { get; set; }
        public Func<ServiceContainer, Type[], object> Factory { get; set; }
        internal ServiceDescription Next { get; set; }

        internal IEnumerable<ServiceDescription> AsEnumerable()
        {
            var list = new List<ServiceDescription>();
            ServiceDescription current = this;
            while (current != null)
            {
                list.Add(current);
                current = current.Next;
            }
            return list;
        }
    }
}