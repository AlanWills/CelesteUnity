using Celeste.Components;

namespace Celeste
{
    public interface IComponentContainerController<TInstance, TComponent> 
        where TInstance : IComponentContainerInstance<TComponent>
        where TComponent : BaseComponent
    {
        TInstance Instance { get; }
        UnityEngine.Transform transform { get; }
        UnityEngine.GameObject gameObject { get; }

        void Hookup(TInstance instance, IRuntimeAddedContext context);
        void Shutdown();
    }
}
