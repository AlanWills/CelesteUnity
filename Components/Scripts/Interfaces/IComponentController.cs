namespace Celeste.Components
{
    public interface IComponentController<T> where T : Component
    {
        void Hookup(IComponentContainerRuntime<T> container, IRuntimeAddedContext context);
        void Shutdown();
    }
}
