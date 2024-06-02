using Celeste.Components.Persistence;
using Celeste.DataStructures;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Components
{
    public class ComponentContainerRuntime<T> : IComponentContainerRuntime<T> where T : BaseComponent
    {
        #region Properties and Fields

        public UnityEvent ComponentDataChanged { get; } = new UnityEvent();

        public int InstanceId { get; }
        public int ParentInstanceId { get; }
        public IComponentContainerController<IComponentContainerRuntime<T>, T> Controller { get; set; }
        public int NumComponents => components.Count;

        private static int sInstanceId;

        [NonSerialized] private List<ComponentHandle<T>> components = new List<ComponentHandle<T>>();

        #endregion

        public ComponentContainerRuntime() : this(0)
        {
        }

        public ComponentContainerRuntime(int parentInstanceId)
        {
            InstanceId = ++sInstanceId;
            ParentInstanceId = parentInstanceId;
        }

        public void InitializeComponents(IComponentContainerUsingSubAssets<T> componentContainer)
        {
            for (int i = 0, n = componentContainer.NumComponents; i < n; ++i)
            {
                AddComponent(componentContainer.GetComponent(i));
            }
        }

        public void InitializeComponents(IComponentContainerUsingTemplates<T> componentContainer)
        {
            for (int i = 0, n = componentContainer.NumComponents; i < n; ++i)
            {
                T component = componentContainer.GetComponent(i);
                ComponentData data = componentContainer.GetComponentData(i);
                ComponentEvents events = component.CreateEvents();
                AddComponent(new ComponentHandle<T>(component, data, events));
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

        public void LoadComponents(List<ComponentDTO> componentDTOs)
        {
            LoadComponents(componentDTOs.ToLookup());
        }

        public void LoadComponents(Dictionary<string, ComponentDTO> componentDTOLookup)
        {
            for (int i = 0, n = NumComponents; i < n; i++)
            {
                var componentHandle = GetComponent(i);

                if (componentDTOLookup.TryGetValue(componentHandle.component.name, out ComponentDTO dto))
                {
                    JsonUtility.FromJsonOverwrite(dto.data, componentHandle.instance.data);
                    componentHandle.component.Load(componentHandle.instance);
                }
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

        public bool TryFindComponent<K>(out ComponentHandle<K> component) where K : T
        {
            foreach (var c in components)
            {
                if (c.Is<K>())
                {
                    component = c.AsComponent<K>();
                    return true;
                }
            }

            component = ComponentHandle<K>.NULL;
            return false;
        }

        public IEnumerable<InterfaceHandle<K>> FindComponents<K>() where K : class
        {
            foreach (var c in components)
            {
                if (c.Is<K>())
                {
                    yield return c.AsInterface<K>();
                }
            }
        }
    }
}
