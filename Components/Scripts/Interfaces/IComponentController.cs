namespace Celeste.Components
{
    public interface IComponentController<T>
        where T : BaseComponent
    {
        bool enabled { get; set; }

        bool IsValidFor(IComponentContainerRuntime<T> container, IRuntimeAddedContext context);
        void Hookup(IComponentContainerRuntime<T> container, IRuntimeAddedContext context);
        void Shutdown();
    }
}
