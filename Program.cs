using System;

namespace MyServiceContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TestRoot();
            TestScope();
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
    }

    interface IFoo { }
    interface IBar { }
    interface IBaz { }
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
}
