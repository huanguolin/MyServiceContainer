using System;
using System.Diagnostics.CodeAnalysis;

namespace MyServiceContainer
{
    public class Key : IEquatable<Key>
    {
        public Key(ServiceDescription serviceDescription, Type[] genericArguments)
        {
            ServiceDescription = serviceDescription;
            GenericArguments = genericArguments;
        }

        public ServiceDescription ServiceDescription { get; }
        public Type[] GenericArguments { get; }

        public bool Equals([AllowNull] Key other)
        {
            if (other == null)
            {
                return false;
            }

            if (ServiceDescription != other.ServiceDescription)
            {
                return false;
            }

            if (GenericArguments.Length != other.GenericArguments.Length)
            {
                return false;
            }

            for (int i = 0; i < GenericArguments.Length; i++)
            {
                if (GenericArguments[i] != other.GenericArguments[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is Key key)
            {
                return Equals(key);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            var hashCode = ServiceDescription.GetHashCode();
            foreach (var item in GenericArguments)
            {
                hashCode ^= item.GetHashCode();
            }
            return hashCode;
        }
    }
}