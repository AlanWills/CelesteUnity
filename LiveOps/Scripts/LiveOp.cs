using Celeste.Components;
using Celeste.DataStructures;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Celeste.LiveOps
{
    public class LiveOp
    {
        #region Properties and Fields

        public UnityEvent<LiveOp> StateChanged { get; } = new UnityEvent<LiveOp>();
        public UnityEvent<LiveOp> DataChanged { get; } = new UnityEvent<LiveOp>();

        public long Type { get; }
        public long SubType { get; }
        public long StartTimestamp { get; }
        public LiveOpState State
        {
            get => liveOpState;
            set
            {
                if (liveOpState != value)
                {
                    liveOpState = value;
                    StateChanged.Invoke(this);
                }
            }
        }

        public int NumComponents => components.Count;

        [NonSerialized] private LiveOpState liveOpState = LiveOpState.ComingSoon;
        [NonSerialized] private List<ComponentHandle> components = new List<ComponentHandle>();

        #endregion

        public LiveOp(long type, long subType, long startTimestamp, LiveOpState state)
        {
            Type = type;
            SubType = subType;
            StartTimestamp = startTimestamp;
            State = state;
        }

        public void AddComponent(ComponentHandle component)
        {
            component.instance.events.ComponentDataChanged.AddListener(OnComponentDataChanged);
            components.Add(component);
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
                components[componentIndex].instance.events.ComponentDataChanged.RemoveListener(OnComponentDataChanged);
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

        public void Complete()
        {
            State = LiveOpState.Completed;
        }

        public void Finish()
        {
            State = LiveOpState.Finished;
        }

        #region Callbacks

        private void OnComponentDataChanged()
        {
            DataChanged.Invoke(this);
        }

        #endregion
    }
}
