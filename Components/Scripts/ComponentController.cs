using UnityEngine;

namespace Celeste.Components
{
    public abstract class ComponentController<TComponent, TRuntime> : MonoBehaviour, IComponentController<TComponent>
        where TComponent : BaseComponent
        where TRuntime : class, IComponentContainerRuntime<TComponent>
    {
        protected TRuntime Runtime { get; private set; }
        protected IComponentContainerController<IComponentContainerRuntime<TComponent>, TComponent> RuntimeController => Runtime.Controller;

        void IComponentController<TComponent>.Hookup(IComponentContainerRuntime<TComponent> container, IRuntimeAddedContext context)
        {
            UnityEngine.Debug.Assert(container is TRuntime, $"Attempting to hookup a container that is not of type {typeof(TRuntime).Name}.");
            Runtime = container as TRuntime;

            DoHookup(context);
        }

        void IComponentController<TComponent>.Shutdown()
        {
            DoShutdown();
        }

        protected abstract void DoHookup(IRuntimeAddedContext context);
        protected abstract void DoShutdown();
    }
}
