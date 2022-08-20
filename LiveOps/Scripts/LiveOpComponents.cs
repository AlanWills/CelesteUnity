using Celeste.Components;
using Celeste.DataStructures;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Celeste.LiveOps
{
    public class LiveOpComponents
    {
        #region Properties and Fields

        public UnityEvent ComponentDataChanged { get; } = new UnityEvent();

        public int NumComponents => components.Count;

        [NonSerialized] private List<ComponentHandle> components = new List<ComponentHandle>();

        #endregion

        public void AddComponent(ComponentHandle component)
        {
            components.Add(component);
            component.instance.events.ComponentDataChanged.AddListener(ComponentDataChanged.Invoke);
        }

        public ComponentHandle GetComponent(int index)
        {
            return components.Get(index);
        }

        public bool HasComponent<T>()
        {
            return components.Exists(x => x.component is T);
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

        public bool TryFindComponent<T>(out InterfaceHandle<T> iFace) where T : class
        {
            foreach (var c in components)
            {
                if (c.Is<T>())
                {
                    iFace = c.AsInterface<T>();
                    return true;
                }
            }

            iFace = new InterfaceHandle<T>();
            return false;
        }
    }
}
