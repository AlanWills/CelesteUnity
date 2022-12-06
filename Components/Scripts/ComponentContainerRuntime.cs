using Celeste.Components.Persistence;
using Celeste.DataStructures;
using System;
using System.Collections.Generic;
using UnityEngine;
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

        public void InitializeComponents(IComponentContainer<T> componentContainer)
        {
            for (int i = 0, n = componentContainer.NumComponents; i < n; ++i)
            {
                AddComponent(componentContainer.GetComponent(i));
            }
        }

        public void ShutdownComponents()
        {
            for (int i = 0, n = components.Count; i < n; ++i)
            {
                var componentHandle = components[i];
                componentHandle.component.Shutdown(componentHandle.instance);
            }
        }

        public void LoadComponents(Dictionary<string, ComponentDTO> componentDTOLookup)
        {
            for (int i = 0, n = NumComponents; i < n; i++)
            {
                var componentHandle = GetComponent(i);

                if (componentDTOLookup.TryGetValue(componentHandle.component.name, out ComponentDTO dto))
                {
                    JsonUtility.FromJsonOverwrite(dto.data, componentHandle.instance.data);
                }
                else
                {
                    Debug.Log($"Looks like a new component was added since the last save.  Will set default values...");
                    componentHandle.component.SetDefaultValues(componentHandle.instance);
                }
            }
        }

        protected void SetComponentDefaultValues()
        {
            for (int i = 0, n = NumComponents; i < n; ++i)
            {
                var componentHandle = GetComponent(i);
                componentHandle.component.SetDefaultValues(componentHandle.instance);
            }
        }

        public void AddComponent(T component)
        {
            ComponentData data = component.CreateData();
            ComponentEvents events = component.CreateEvents();
            ComponentHandle<T> handle = new ComponentHandle<T>(component, data, events);

            component.Initialize(handle.instance);
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

        public bool HasComponent<K>() where K : class
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

        public ComponentHandle<K> FindComponent<K>() where K : T
        {
            for (int i = 0, n = components.Count; i < n; ++i)
            {
                if (components[i].Is<K>())
                {
                    return components[i].AsComponent<K>();
                }
            }

            return new ComponentHandle<K>();
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
