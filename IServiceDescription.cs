using System;
using System.Collections.Generic;
using System.Linq;

namespace MyServiceContainer
{
    public interface IServiceDescription
    {
        Type ServiceType { get; set; }
        ServiceLifetime Lifetime { get; set; }
        Func<IServiceProvider, Type[], object> Factory { get; set; }
    }
}