using Celeste.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Celeste.LiveOps
{
    public class LiveOp : IEquatable<LiveOp>
    {
        #region Properties and Fields

        public static readonly LiveOp NULL = new LiveOp(
            0, 
            0, 
            0,
            false,
            -1,
            LiveOpState.Unknown,
            new LiveOpComponents(),
            LiveOpConstants.NO_TIMER,
            LiveOpConstants.NO_PROGRESS,
            LiveOpConstants.NO_ASSETS);

        public UnityEvent<LiveOp> StateChanged { get; } = new UnityEvent<LiveOp>();
        public UnityEvent<LiveOp> ProgressChanged { get; } = new UnityEvent<LiveOp>();
        public UnityEvent<LiveOp> DataChanged { get; } = new UnityEvent<LiveOp>();

        public InterfaceHandle<ILiveOpTimer> Timer { get; }
        public InterfaceHandle<ILiveOpProgress> Progress { get; }
        public InterfaceHandle<ILiveOpAssets> Assets { get; }

        public long Type { get; }
        public long SubType { get; }
        public long StartTimestamp { get; }
        public bool IsRecurring { get; }
        public long RepeatsAfter { get; }
        public bool CanSchedule
        {
            get
            {
                if (!Assets.iFace.IsLoaded)
                {
                    return false;
                }

                for (int i = 0, n = NumComponents; i < n; ++i)
                {
                    var component = GetComponent(i);

                    if (component.Is<ILiveOpScheduleCondition>() &&
                        !component.AsInterface<ILiveOpScheduleCondition>().iFace.CanSchedule(component.instance, Assets))
                    {
                        // Schedule condition found and it was not valid
                        return false;
                    }
                }

                // Assets loaded and all conditions true!
                return true;
            }
        }

        public int NumComponents => Components.NumComponents;
        public long EndTimestamp => Timer.iFace.GetEndTimestamp(Timer.instance, StartTimestamp);
        public float ProgressRatio => Progress.iFace.ProgressRatio(Progress.instance);
        public bool PlayerActionRequired => Progress.iFace.PlayerActionRequired(Progress.instance);
        public LiveOpState State
        {
            get => liveOpState;
            private set
            {
                if (liveOpState != value)
                {
                    liveOpState = value;
                    StateChanged.Invoke(this);
                }
            }
        }

        private LiveOpComponents Components { get; }

        private LiveOpState liveOpState;

        #endregion

        public LiveOp(
            long type, 
            long subType, 
            long startTimestamp,
            bool isRecurring,
            long repeatsAfter,
            LiveOpState liveOpState,
            LiveOpComponents components,
            InterfaceHandle<ILiveOpTimer> timer,
            InterfaceHandle<ILiveOpProgress> progress,
            InterfaceHandle<ILiveOpAssets> assets)
        {
            Type = type;
            SubType = subType;
            StartTimestamp = startTimestamp;
            IsRecurring = isRecurring;
            RepeatsAfter = repeatsAfter;
            State = liveOpState;
            Components = components;
            Timer = timer;
            Progress = progress;
            Assets = assets;

            Progress.instance.events.ComponentDataChanged.AddListener(OnProgressComponentDataChanged);
            Components.ComponentDataChanged.AddListener(OnDataChanged);

            UnityEngine.Debug.Assert(!isRecurring || repeatsAfter > 0, $"Found a recurring liveop with an invalid repeats after frequency ({type}, {subType}).");
        }

        public IEnumerator Load()
        {
            yield return Assets.iFace.LoadAssets(Assets.instance);

            if (!Assets.iFace.IsLoaded)
            {
                UnityEngine.Debug.LogError($"Live Op with type {Type} starting at timestamp {StartTimestamp} failed to load its assets, so will not be scheduled.");
                yield break;
            }

            for (int i = 0, n = Components.NumComponents; i < n; i++)
            {
                var component = Components.GetComponent(i);

                // Load all our other components that require assets now that our assets interface is loaded
                if (component.Is<IRequiresAssets>())
                {
                    yield return component.AsInterface<IRequiresAssets>().iFace.Load(Assets);
                }
            }
        }

        public void Start()
        {
            State = LiveOpState.Running;

            if (IsRecurring)
            {
                // Reset the progress of this recurring liveop
                Progress.iFace.ResetProgress(Progress.instance);
            }
        }

        public void Complete()
        {
            State = LiveOpState.Completed;
        }

        public void Finish()
        {
            State = LiveOpState.Finished;
        }

        public ComponentHandle<Component> GetComponent(int index)
        {
            return Components.GetComponent(index);
        }

        public bool HasComponent<T>() where T : class
        {
            return Components.HasComponent<T>();
        }

        public bool TryFindComponent<T>(out InterfaceHandle<T> iFace) where T : class
        {
            return Components.TryFindComponent(out iFace);
        }

        #region Operators

        public override bool Equals(object obj)
        {
            return obj is LiveOp wrapper && Equals(wrapper);
        }

        public bool Equals(LiveOp other)
        {
            return other != null && 
                Type == other.Type &&
                SubType == other.SubType &&
                StartTimestamp == other.StartTimestamp &&
                EndTimestamp == other.EndTimestamp &&
                EqualityComparer<LiveOpComponents>.Default.Equals(Components, other.Components);
        }

        public override int GetHashCode()
        {
            return 1596229712 + 
                Type.GetHashCode() +
                SubType.GetHashCode() + 
                StartTimestamp.GetHashCode() + 
                EndTimestamp.GetHashCode() +
                EqualityComparer<LiveOpComponents>.Default.GetHashCode(Components);
        }

        public static bool operator ==(LiveOp left, LiveOp right)
        {
            return ((object)left == null && (object)right == null) || left.Equals(right);
        }

        public static bool operator !=(LiveOp left, LiveOp right)
        {
            return !(left == right);
        }

        #endregion

        #region Callbacks

        private void OnProgressComponentDataChanged()
        {
            ProgressChanged.Invoke(this);
        }

        private void OnDataChanged()
        {
            DataChanged.Invoke(this);
        }

        #endregion
    }
}
