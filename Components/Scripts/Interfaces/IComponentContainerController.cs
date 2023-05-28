using Celeste.Components;

namespace Celeste
{
    public interface IComponentContainerController<TRuntime, TComponent> 
        where TRuntime : IComponentContainerRuntime<TComponent>
        where TComponent : Component
    {
        TRuntime Runtime { get; }
        UnityEngine.Transform transform { get; }
    }
}
