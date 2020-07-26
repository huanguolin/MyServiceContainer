namespace MyServiceContainer
{
    public interface IServiceContainer
    {
        IServiceProvider CreateScopeContainer(IServiceContainer root);
    }
}