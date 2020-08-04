using System;
using System.Collections.Generic;

namespace MyServiceContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TestRoot();
            // TestScope();
            // TestGenericArgs();
            TestEnumerable();
        }

        static void TestRoot()
        {
            using var container = new ServiceContainer();

            container.AddTransient<IFoo, Foo>();
            container.AddSingleton<IBar, Bar>();
            container.AddScope<IBaz, Baz>();

            container.GetService<IFoo>();
            container.GetService<IFoo>();
            container.GetService<IBar>();
            container.GetService<IBar>();
            container.GetService<IBaz>();
            container.GetService<IBaz>();
        }

        static void TestScope()
        {
            var container = new ServiceContainer();

            using (var scopeContainer = container.CreateScopeContainer())
            {
                container.AddTransient<IFoo, Foo>();
                container.AddSingleton<IBar, Bar>();
                container.AddScope<IBaz, Baz>();

                container.GetService<IFoo>();
                container.GetService<IFoo>();
                container.GetService<IBar>();
                container.GetService<IBar>();
                container.GetService<IBaz>();
                container.GetService<IBaz>();
            }

            container.GetService<IFoo>();
            container.GetService<IBar>();
        }

        static void TestGenericArgs()
        {
            var container = new ServiceContainer();
            using (var scopeContainer = container.CreateScopeContainer())
            {
                container.AddScope<IFoo, Foo>();
                container.AddScope<IBar, Bar>();
                container.AddScope<IBaz, Baz>();

                container.AddService(typeof(IQux<IFoo>), typeof(Qux<>), ServiceLifetime.Scope);
                container.AddService(typeof(IQux<IBar>), typeof(Qux<>), ServiceLifetime.Scope);
                container.AddService(typeof(IQux<IBaz>), typeof(Qux<>), ServiceLifetime.Scope);

                container.GetService<IQux<IFoo>>();
                container.GetService<IQux<IBar>>();
                container.GetService<IQux<IBaz>>();
            }
        }

        static void TestEnumerable()
        {
            var container = new ServiceContainer();

            container.AddSingleton<IBaz, Baz>();
            container.AddSingleton<IBaz, Baz2>();

            container.GetService<IEnumerable<IBaz>>();
        }
    }

    interface IFoo { }
    interface IBar { }
    interface IBaz { }
    interface IQux<T> {}
    class Foo : IFoo, IDisposable
    {
        public Foo()
        {
            Console.WriteLine("Foo create.");
        }

        public void Dispose()
        {
            Console.WriteLine("Foo dispose.");
        }
    }
    class Bar : IBar, IDisposable
    {
        public Bar()
        {
            Console.WriteLine("Bar create.");
        }

        public void Dispose()
        {
            Console.WriteLine("Bar dispose.");
        }
    }
    class Baz : IBaz, IDisposable
    {
        public Baz()
        {
            Console.WriteLine("Baz create.");
        }

        public void Dispose()
        {
            Console.WriteLine("Baz dispose.");
        }
    }
    class Baz2 : IBaz, IDisposable
    {
        public Baz2()
        {
            Console.WriteLine("Baz2 create.");
        }

        public void Dispose()
        {
            Console.WriteLine("Baz2 dispose.");
        }
    }
    class Qux<T> : IQux<T>, IDisposable
    {
        public Qux(T t)
        {
            Console.WriteLine($"Qux create and get {t.GetType()}.");
        }

        public void Dispose()
        {
            Console.WriteLine("Qux dispose.");
        }
    }
}
