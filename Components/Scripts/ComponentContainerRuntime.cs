using Celeste.DataStructures;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Celeste.Components
{
    public class ComponentContainerRuntime<T> : IComponentContainerRuntime<T> where T : Component
    {
        #region Properties and Fields

        public UnityEvent ComponentDataChanged { get; } = new UnityEvent();

        public int NumComponents => components.Count;

        [NonSerialized] private List<ComponentHandle<T>> components = new List<ComponentHandle<T>>();

        #endregion

        public void InitComponents(IComponentContainer<T> componentContainer)
        {
            for (int i = 0, n = componentContainer.NumComponents; i < n; ++i)
            {
                AddComponent(componentContainer.GetComponent(i));
            }
        }

        public void AddComponent(T component)
        {
            ComponentData data = component.CreateData();
            ComponentEvents events = component.CreateEvents();
            ComponentHandle<T> handle = new ComponentHandle<T>(component, data, events);
            components.Add(handle);
            events.ComponentDataChanged.AddListener(ComponentDataChanged.Invoke);
        }

        public void AddComponent(ComponentHandle<T> componentHandle)
        {
            components.Add(componentHandle);
            componentHandle.instance.events.ComponentDataChanged.AddListener(ComponentDataChanged.Invoke);
        }

        public ComponentHandle<T> GetComponent(int index)
        {
            return components.Get(index);
        }

        public bool HasComponent<K>() where K : T
        {
            return components.Exists(x => x.component is K);
        }

        public void RemoveComponent(int componentIndex)
        {
#if INDEX_CHECKS
            if (0 <= componentIndex && componentIndex < NumComponents)
#endif
            {
                components[componentIndex].instance.events.ComponentDataChanged.RemoveListener(ComponentDataChanged.Invoke);
                components.RemoveAt(componentIndex);
            }
        }

        public bool TryFindComponent<K>(out InterfaceHandle<K> iFace) where K : class
        {
            foreach (var c in components)
            {
                if (c.Is<K>())
                {
                    iFace = c.AsInterface<K>();
                    return true;
                }
            }

            iFace = InterfaceHandle<K>.NULL;
            return false;
        }
    }
}
