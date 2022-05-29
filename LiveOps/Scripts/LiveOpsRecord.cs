using Celeste.Components;
using Celeste.Components.Catalogue;
using Celeste.Components.Persistence;
using Celeste.Core;
using Celeste.DataStructures;
using Celeste.LiveOps.Persistence;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.LiveOps
{
    [CreateAssetMenu(fileName = nameof(LiveOpsRecord), menuName = "Celeste/Live Ops/Live Ops Record")]
    public class LiveOpsRecord : ScriptableObject
    {
        #region LiveOpWrapper

        private struct LiveOpWrapper : IEquatable<LiveOpWrapper>
        {
            public static readonly LiveOpWrapper NULL = new LiveOpWrapper();

            public LiveOp LiveOp { get; }
            public InterfaceHandle<ILiveOpTimer> Timer { get; }
            public InterfaceHandle<ILiveOpProgress> Progress { get; }

            public long Type => LiveOp.Type;
            public long StartTimestamp => LiveOp.StartTimestamp;
            public long EndTimestamp => Timer.iFace.GetEndTimestamp(Timer.instance);
            public LiveOpState State { get => LiveOp.State; set => LiveOp.State = value; }
            public bool HasProgress => Progress.iFace.HasProgress(Progress.instance);

            public LiveOpWrapper(LiveOp liveOp, InterfaceHandle<ILiveOpTimer> timer, InterfaceHandle<ILiveOpProgress> progress)
            {
                LiveOp = liveOp;
                Timer = timer;
                Progress = progress;
            }

            public override bool Equals(object obj)
            {
                return obj is LiveOpWrapper wrapper && Equals(wrapper);
            }

            public bool Equals(LiveOpWrapper other)
            {
                return EqualityComparer<LiveOp>.Default.Equals(LiveOp, other.LiveOp);
            }

            public override int GetHashCode()
            {
                return 1596229712 + EqualityComparer<LiveOp>.Default.GetHashCode(LiveOp);
            }

            public static bool operator ==(LiveOpWrapper left, LiveOpWrapper right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(LiveOpWrapper left, LiveOpWrapper right)
            {
                return !(left == right);
            }
        }

        #endregion

        #region Properties and Fields

        public int NumLiveOps => liveOps.Count;

        [Header("Dependencies")]
        [SerializeField] private ComponentCatalogue liveOpsComponentCatalogue;
        [SerializeField] private ScheduledCallbacks scheduledCallbacks;

        [Header("Events")]
        [SerializeField] private Events.Event save;

        [NonSerialized] private List<LiveOpWrapper> liveOps = new List<LiveOpWrapper>();

        #endregion

        public void AddLiveOp(LiveOpDTO liveOpDTO)
        {
            if (liveOps.Exists(x => x.Type == liveOpDTO.type && x.StartTimestamp == liveOpDTO.startTimestamp))
            {
                UnityEngine.Debug.Log($"Live Op with id {liveOpDTO.type} starting at timestamp {liveOpDTO.startTimestamp} is already running.");
                return;
            }

            LiveOp liveOp = new LiveOp(liveOpDTO.type, liveOpDTO.startTimestamp, (LiveOpState)liveOpDTO.state);

            foreach (ComponentDTO componentDTO in liveOpDTO.components)
            {
                ComponentHandle componentHandle = liveOpsComponentCatalogue.CreateComponent(componentDTO.typeName, componentDTO.data);
                
                if (componentHandle.IsValid)
                {
                    liveOp.AddComponent(componentHandle);
                }
            }

            if (!liveOp.TryFindComponent<ILiveOpTimer>(out var timer))
            {
                UnityEngine.Debug.LogAssertion($"Live Op with type {liveOp.Type} starting at timestamp {liveOp.StartTimestamp} has no {nameof(ILiveOpTimer)} component, so will not be scheduled.");
                return;
            }

            if (!liveOp.TryFindComponent<ILiveOpProgress>(out var progress))
            {
                UnityEngine.Debug.LogAssertion($"Live Op with type {liveOp.Type} starting at timestamp {liveOp.StartTimestamp} has no {nameof(ILiveOpProgress)} component, so will not be scheduled.");
                return;
            }

            if (liveOp.State == LiveOpState.Finished)
            {
                UnityEngine.Debug.Log($"Live Op with type {liveOp.Type} starting at timestamp {liveOp.StartTimestamp} has finished, so will not be added.");
                return;
            }

            LiveOpWrapper liveOpWrapper = new LiveOpWrapper(liveOp, timer, progress);
            timer.iFace.SetStartTimestamp(timer.instance, liveOp.StartTimestamp);
            liveOp.StateChanged.AddListener(OnLiveOpStateChanged);
            liveOps.Add(liveOpWrapper);

            Schedule(liveOpWrapper);
        }

        public LiveOp GetLiveOp(int index)
        {
            return liveOps.Get(index).LiveOp;
        }

        private void Schedule(LiveOpWrapper liveOp)
        {
            switch (liveOp.State)
            {
                case LiveOpState.ComingSoon:
                    HandleScheduleOfComingSoonLiveOp(liveOp);
                    break;

                case LiveOpState.Running:
                    HandleScheduleOfRunningLiveOp(liveOp);
                    break;

                case LiveOpState.Completed:
                    HandleScheduleOfCompletedLiveOp(liveOp);
                    break;

                case LiveOpState.Finished:
                    HandleScheduleOfFinishedLiveOp(liveOp);
                    break;

                default:
                    UnityEngine.Debug.LogAssertion($"Unhandled {nameof(LiveOpState)} {liveOp.State}.");
                    break;
            }
        }

        private void HandleScheduleOfComingSoonLiveOp(LiveOpWrapper liveOp)
        {
            if (liveOp.StartTimestamp <= GameTime.Now)
            {
                // We can actually start the live op now, so let's do it!
                liveOp.State = LiveOpState.Running;

                scheduledCallbacks.Schedule(liveOp.EndTimestamp, () => Schedule(liveOp));
            }
            else
            {
                // Schedule a callback to start the event at the appropriate timestamp
                scheduledCallbacks.Schedule(liveOp.StartTimestamp, () => Schedule(liveOp));
            }
        }

        private void HandleScheduleOfRunningLiveOp(LiveOpWrapper liveOp)
        {
            long endTime = liveOp.EndTimestamp;

            if (endTime < GameTime.Now)
            {
                // Event has timed out - use the progress interface to see if we can just dismiss the event
                if (!liveOp.HasProgress)
                {
                    // We can immediately end this event now as the player has no unresolved progress
                    liveOp.State = LiveOpState.Finished;
                }
            }
            else
            {
                // Schedule a callback to handle the timeout of the event
                scheduledCallbacks.Schedule(endTime, () => Schedule(liveOp));
            }
        }

        private void HandleScheduleOfCompletedLiveOp(LiveOpWrapper liveOp)
        {
            if (!liveOp.HasProgress)
            {
                // We can immediately end this event now, as the player has no unresolved progress
                liveOp.State = LiveOpState.Finished;
            }
        }

        private void HandleScheduleOfFinishedLiveOp(LiveOpWrapper liveOp)
        {
            // Nothing we need to do right now - in fact, we shouldn't even get here because finished live ops are removed on start
            // and if they've become finished during game time, we shouldn't reschedule them
        }

        #region Callbacks

        private void OnLiveOpStateChanged(LiveOp liveOp)
        {
            save.Invoke();
        }

        #endregion
    }
}
