using UnityEngine;

namespace Celeste.Components
{
    public abstract class ComponentController<TComponent, TBaseComponent, TRuntime> : MonoBehaviour, IComponentController<TBaseComponent>
        where TComponent : TBaseComponent
        where TBaseComponent : BaseComponent
        where TRuntime : class, IComponentContainerRuntime<TBaseComponent>
    {
        protected ComponentHandle<TComponent> Component { get; private set; }
        protected TRuntime Runtime { get; private set; }
        protected IComponentContainerController<IComponentContainerRuntime<TBaseComponent>, TBaseComponent> RuntimeController => Runtime.Controller;

        bool IComponentController<TBaseComponent>.IsValidFor(
            IComponentContainerRuntime<TBaseComponent> container,
            IRuntimeAddedContext context)
        {
            return container.TryFindComponent(out ComponentHandle<TComponent> componentHandle) && CheckIsValidFor(componentHandle, container, context);
        }
        
        void IComponentController<TBaseComponent>.Hookup(
            IComponentContainerRuntime<TBaseComponent> container, 
            IRuntimeAddedContext context)
        {
            bool didFindComponent = container.TryFindComponent(out ComponentHandle<TComponent> componentHandle);
            Debug.Assert(didFindComponent, "Attempting to hookup a component controller that is not being given a valid component handle.");
            Component = componentHandle;
            
            Debug.Assert(container is TRuntime, $"Attempting to hookup a container that is not of type {typeof(TRuntime).Name}.");
            Runtime = container as TRuntime;

            DoHookup(context);
        }

        void IComponentController<TBaseComponent>.Shutdown()
        {
            DoShutdown();
            
            Component = ComponentHandle<TComponent>.NULL;
            Runtime = null;
        }

        protected virtual bool CheckIsValidFor(
            ComponentHandle<TComponent> componentHandle,
            IComponentContainerRuntime<TBaseComponent> container, 
            IRuntimeAddedContext context) => true;
        protected abstract void DoHookup(IRuntimeAddedContext context);
        protected abstract void DoShutdown();
    }
}
