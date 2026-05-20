namespace Celeste.Components
{
    public interface IComponentController<T>
        where T : BaseComponent
    {
        bool enabled { get; set; }

        bool IsValidFor(IComponentContainerInstance<T> container, IRuntimeAddedContext context);
        void Hookup(IComponentContainerInstance<T> container, IRuntimeAddedContext context);
        void Shutdown();
    }
}
