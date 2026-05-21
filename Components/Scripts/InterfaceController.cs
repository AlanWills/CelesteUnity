using UnityEngine;

namespace Celeste.Components
{
    public abstract class InterfaceController<TInterface, TBaseComponent, TInstance> : MonoBehaviour, IComponentController<TBaseComponent>
        where TBaseComponent : BaseComponent
        where TInstance : class, IComponentContainerInstance<TBaseComponent>
        where TInterface : class
    {
        public InterfaceHandle<TInterface> Interface { get; private set; }
        public TInstance Instance { get; private set; }

        bool IComponentController<TBaseComponent>.IsValidFor(
            IComponentContainerInstance<TBaseComponent> container,
            IRuntimeAddedContext context)
        {
            return container.TryFindComponent(out InterfaceHandle<TInterface> interfaceHandle) && CheckIsValidFor(interfaceHandle, container, context);
        }
        
        void IComponentController<TBaseComponent>.Hookup(
            IComponentContainerInstance<TBaseComponent> container, 
            IRuntimeAddedContext context)
        {
            bool didFindInterface = container.TryFindComponent(out InterfaceHandle<TInterface> interfaceHandle);
            Debug.Assert(didFindInterface, "Attempting to hookup an interface controller that is not being given a valid interface handle.");
            Interface = interfaceHandle;
            
            Debug.Assert(container is TInstance, $"Attempting to hookup a container that is not of type {typeof(TInstance).Name}.");
            Instance = container as TInstance;
            
            DoHookup(context);
            
            Interface.instance.events.ComponentDataChanged.AddListener(OnInterfaceDataChanged);
        }

        void IComponentController<TBaseComponent>.Shutdown()
        {
            if (Interface.IsValid)
            {
                Interface.instance.events.ComponentDataChanged.RemoveListener(OnInterfaceDataChanged);
            }

            DoShutdown();
            
            Interface = InterfaceHandle<TInterface>.NULL;
            Instance = null;
        }

        protected virtual bool CheckIsValidFor(
            InterfaceHandle<TInterface> interfaceHandle,
            IComponentContainerInstance<TBaseComponent> container, 
            IRuntimeAddedContext context) => true;
        protected abstract void DoHookup(IRuntimeAddedContext context);
        protected abstract void DoShutdown();
        
        protected virtual void OnInterfaceDataChanged() { }
    }
}
