using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Components
{
    public class ComponentContainerController<TComponent, TRuntime> : MonoBehaviour, IComponentContainerController<IComponentContainerRuntime<TComponent>, TComponent>
        where TRuntime : IComponentContainerRuntime<TComponent> 
        where TComponent : BaseComponent
    {
        #region Properties and Fields

        IComponentContainerRuntime<TComponent> IComponentContainerController<IComponentContainerRuntime<TComponent>, TComponent>.Runtime => Runtime;

        public TRuntime Runtime { get; private set; }

        [SerializeField] private List<GameObject> componentControllers = new List<GameObject>();

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            componentControllers.Clear();

            foreach (var componentController in GetComponentsInChildren<IComponentController<TComponent>>())
            {
                componentControllers.Add((componentController as MonoBehaviour).gameObject);
            }
        }

        #endregion

        public void Hookup(TRuntime runtime, IRuntimeAddedContext context)
        {
            Runtime = runtime;
            Runtime.Controller = this;

            foreach (var componentControllerGameObject in componentControllers)
            {
                var componentController = componentControllerGameObject.GetComponent<IComponentController<TComponent>>();

                if (componentController.IsValidFor(runtime, context))
                {
                    componentController.Hookup(runtime, context);
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
                if (component != null)
                {
                    component.GetComponent<IComponentController<TComponent>>().Shutdown();
                }
            }

            Runtime = default;
        }
    }
}
