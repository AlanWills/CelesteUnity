﻿using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Components
{
    public class ComponentContainerController<TRuntime, TComponent> : MonoBehaviour, IComponentContainerController<IComponentContainerRuntime<TComponent>, TComponent>
        where TRuntime : IComponentContainerRuntime<TComponent> 
        where TComponent : Component
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

            foreach (var componentController in GetComponents<IComponentController<TComponent>>())
            {
                componentControllers.Add((componentController as MonoBehaviour).gameObject);
            }
        }

        #endregion

        public void Hookup(TRuntime runtime, IRuntimeAddedContext context)
        {
            Runtime = runtime;
            Runtime.Controller = this;

            foreach (var component in componentControllers)
            {
                component.GetComponent<IComponentController<TComponent>>().Hookup(runtime, context);
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
