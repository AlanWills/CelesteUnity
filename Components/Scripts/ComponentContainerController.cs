using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Components
{
    public class ComponentContainerController<TInstance, TComponent> : MonoBehaviour, IComponentContainerController<TInstance, TComponent>
        where TInstance : IComponentContainerInstance<TComponent> 
        where TComponent : BaseComponent
    {
        #region Properties and Fields

        public TInstance Instance { get; private set; }

        [SerializeField] private List<Component> componentControllers = new();

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            componentControllers.Clear();

            foreach (var componentController in GetComponentsInChildren<IComponentController<TComponent>>())
            {
                componentControllers.Add(componentController as Component);
            }
        }

        #endregion

        public void Hookup(TInstance instance, IRuntimeAddedContext context)
        {
            Instance = instance;

            foreach (var component in componentControllers)
            {
                var componentController = component as IComponentController<TComponent>;

                if (componentController.IsValidFor(instance, context))
                {
                    componentController.Hookup(instance, context);
                    componentController.enabled = true;
                }
                else
                {
                    componentController.Shutdown();
                    componentController.enabled = false;
                }
            }
        }

        public void Shutdown()
        {
            foreach (var component in componentControllers)
            {
                (component as IComponentController<TComponent>)?.Shutdown();
            }

            Instance = default;
        }
    }
}
