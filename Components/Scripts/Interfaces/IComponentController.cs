namespace Celeste.Components
{
    public interface IComponentController<T> where T : BaseComponent
    {
        void Hookup(IComponentContainerRuntime<T> container, IRuntimeAddedContext context);
        void Shutdown();
    }
}
