using Celeste.Components;
using Celeste.Components.Catalogue;
using Celeste.Components.Persistence;
using Celeste.Core;
using Celeste.DataStructures;
using Celeste.Events;
using Celeste.LiveOps.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.LiveOps
{
    [CreateAssetMenu(fileName = nameof(LiveOpsRecord), menuName = "Celeste/Live Ops/Live Ops Record")]
    public class LiveOpsRecord : ScriptableObject
    {
        #region LiveOpWrapper

        private class LiveOpWrapper : IEquatable<LiveOpWrapper>
        {
            public static readonly LiveOpWrapper NULL = new LiveOpWrapper(
                new LiveOp(0, 0, 0, LiveOpState.Unknown),
                LiveOpConstants.NO_TIMER,
                LiveOpConstants.NO_PROGRESS,
                LiveOpConstants.NO_ASSETS);

            public LiveOp LiveOp { get; }
            public InterfaceHandle<ILiveOpTimer> Timer { get; }
            public InterfaceHandle<ILiveOpProgress> Progress { get; }
            public InterfaceHandle<ILiveOpAssets> Assets { get; }

            public long Type => LiveOp.Type;
            public long SubType => LiveOp.SubType;
            public long StartTimestamp => LiveOp.StartTimestamp;
            public long EndTimestamp => Timer.iFace.GetEndTimestamp(Timer.instance, StartTimestamp);
            public LiveOpState State { get => LiveOp.State; set => LiveOp.State = value; }
            public bool HasProgress => Progress.iFace.HasProgress(Progress.instance);

            public LiveOpWrapper(
                LiveOp liveOp, 
                InterfaceHandle<ILiveOpTimer> timer, 
                InterfaceHandle<ILiveOpProgress> progress,
                InterfaceHandle<ILiveOpAssets> assets)
            {
                LiveOp = liveOp;
                Timer = timer;
                Progress = progress;
                Assets = assets;
            }

            public IEnumerator Load()
            {
                yield return Assets.iFace.Load(Assets.instance);

                if (!Assets.iFace.IsLoaded)
                {
                    UnityEngine.Debug.LogError($"Live Op with type {Type} starting at timestamp {StartTimestamp} failed to load its assets, so will not be scheduled.");
                    yield break;
                }
                
                for (int i = 0, n = LiveOp.NumComponents; i < n; i++)
                {
                    var component = LiveOp.GetComponent(i);

                    // Load all our other components that require assets now that our assets interface is loaded
                    if (component.Is<IRequiresAssets>())
                    {
                        yield return component.AsInterface<IRequiresAssets>().iFace.Load(Assets);
                    }
                }
            }

            public void Complete()
            {
                LiveOp.Complete();
            }

            public void Finish()
            {
                LiveOp.Finish();
            }

            #region Operators

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

            #endregion
        }

        #endregion

        #region Properties and Fields

        public int NumLiveOps => liveOps.Count;

        [Header("Dependencies")]
        [SerializeField] private ComponentCatalogue liveOpsComponentCatalogue;
        [SerializeField] private ScheduledCallbacks scheduledCallbacks;

        [Header("Events")]
        [SerializeField] private LiveOpEvent liveOpAdded;
        [SerializeField] private LiveOpEvent liveOpStateChanged;
        [SerializeField] private Events.Event save;

        [NonSerialized] private List<LiveOpWrapper> liveOps = new List<LiveOpWrapper>();
        [NonSerialized] private List<ValueTuple<LiveOpWrapper, CallbackHandle>> scheduleCallbackHandles = new List<ValueTuple<LiveOpWrapper, CallbackHandle>>();

        #endregion

        public LiveOp GetLiveOp(int index)
        {
            return liveOps.Get(index).LiveOp;
        }

        public IEnumerator AddLiveOp(LiveOpDTO liveOpDTO)
        {
            if (liveOps.Exists(x =>
                x.Type == liveOpDTO.type && 
                x.SubType == liveOpDTO.subType))
            {
                UnityEngine.Debug.Log($"Live Op with id {liveOpDTO.type} starting at timestamp {liveOpDTO.startTimestamp} is already running.");
                yield break;
            }

            long liveOpStartTimestamp = liveOpDTO.startTimestamp;
            LiveOpState liveOpState = (LiveOpState)liveOpDTO.state;

            if (liveOpState == LiveOpState.Unknown || liveOpState == LiveOpState.Finished)
            {
                UnityEngine.Debug.Log($"Live Op with type {liveOpDTO.type} starting at timestamp {liveOpDTO.startTimestamp} has finished, so will not be added.");

                if (!liveOpDTO.isRecurring)
                {
                    // This live op is not recurring so we will not add it
                    yield break;
                }

                // Calculate the latest possible start timestamp in the past based on the liveop start timestamp and the recurrence frequency
                long diffBetweenNowAndStart = GameTime.Now - liveOpStartTimestamp;
                liveOpStartTimestamp = GameTime.Now - (diffBetweenNowAndStart % liveOpDTO.repeatsAfter);

                // Set the state to ComingSoon, so it'll be handled properly when we schedule - we can't do more without the timer
                liveOpState = LiveOpState.ComingSoon;
            }

            LiveOp liveOp = new LiveOp(
                liveOpDTO.type, 
                liveOpDTO.subType,
                liveOpStartTimestamp,
                liveOpState);

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
                yield break;
            }

            if (!liveOp.TryFindComponent<ILiveOpProgress>(out var progress))
            {
                UnityEngine.Debug.LogAssertion($"Live Op with type {liveOp.Type} starting at timestamp {liveOp.StartTimestamp} has no {nameof(ILiveOpProgress)} component, so will not be scheduled.");
                yield break;
            }

            if (!liveOp.TryFindComponent<ILiveOpAssets>(out var assets))
            {
                UnityEngine.Debug.LogAssertion($"Live Op with type {liveOp.Type} starting at timestamp {liveOp.StartTimestamp} has no {nameof(ILiveOpAssets)} component, so will not be scheduled.");
                yield break;
            }

            LiveOpWrapper liveOpWrapper = new LiveOpWrapper(liveOp, timer, progress, assets);

            // Now we've grabbed all the components and set up the data, we can calculate the initial state of the live op
            if (liveOpWrapper.StartTimestamp > GameTime.Now)
            {
                // Start time is still in the future
                liveOpWrapper.State = LiveOpState.ComingSoon;
            }
            else if (liveOpWrapper.EndTimestamp > GameTime.Now &&
                (liveOpWrapper.State == LiveOpState.ComingSoon || liveOpWrapper.State == LiveOpState.Unknown))
            {
                // We had a live op that was not started previously, so we can start it now as we're in the running time
                liveOpWrapper.State = LiveOpState.Running;
            }
            else if (liveOpWrapper.EndTimestamp <= GameTime.Now &&
                !progress.iFace.HasProgress(progress.instance))
            {
                // The end time was in the past and we have no progress so we can just finish this live op
                liveOpWrapper.State = LiveOpState.Finished;
            }

            yield return liveOpWrapper.Load();

            if (liveOpWrapper.Assets.iFace.IsLoaded)
            {
                liveOp.StateChanged.AddListener(OnLiveOpStateChanged);
                liveOp.DataChanged.AddListener(OnLiveOpDataChanged);
                liveOps.Add(liveOpWrapper);

                Schedule(liveOpWrapper);

                liveOpAdded.Invoke(liveOp);
            }
        }

        #region Scheduling

        private void Schedule(LiveOpWrapper liveOp)
        {
            int callbackIndex = scheduleCallbackHandles.FindIndex(x => x.Item1.LiveOp == liveOp.LiveOp);
            if (callbackIndex >= 0)
            {
                // Cancel our current schedule callback if we have one, ready for our new scheduling
                scheduledCallbacks.Cancel(scheduleCallbackHandles[callbackIndex].Item2);
                scheduleCallbackHandles.RemoveAt(callbackIndex);
            }

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
                if (liveOp.EndTimestamp > GameTime.Now)
                {
                    // We can actually start the live op now, so let's do it!
                    liveOp.State = LiveOpState.Running;

                    CallbackHandle callbackHandle = scheduledCallbacks.Schedule(liveOp.EndTimestamp, () => Schedule(liveOp));
                    scheduleCallbackHandles.Add((liveOp, callbackHandle));
                }
                else
                {
                    // This live op was over before it even began, so let's finish it
                    liveOp.Finish();
                }
            }
            else
            {
                // Schedule a callback to start the event at the appropriate timestamp
                CallbackHandle callbackHandle = scheduledCallbacks.Schedule(liveOp.StartTimestamp, () => Schedule(liveOp));
                scheduleCallbackHandles.Add((liveOp, callbackHandle));
            }
        }

        private void HandleScheduleOfRunningLiveOp(LiveOpWrapper liveOp)
        {
            long endTime = liveOp.EndTimestamp;

            if (endTime <= GameTime.Now)
            {
                // Event has timed out - use the progress interface to see if we can just dismiss the event
                if (!liveOp.HasProgress)
                {
                    // We can immediately end this event now as the player has no unresolved progress
                    liveOp.Finish();
                }
            }
            else
            {
                // Schedule a callback to handle the timeout of the event
                CallbackHandle callbackHandle = scheduledCallbacks.Schedule(endTime, () => Schedule(liveOp));
                scheduleCallbackHandles.Add((liveOp, callbackHandle));
            }
        }

        private void HandleScheduleOfCompletedLiveOp(LiveOpWrapper liveOp)
        {
            if (!liveOp.HasProgress)
            {
                // We can immediately end this event now, as the player has no unresolved progress
                liveOp.Finish();
            }
        }

        private void HandleScheduleOfFinishedLiveOp(LiveOpWrapper liveOp)
        {
            // Nothing we need to do right now - in fact, we shouldn't even get here because finished live ops are removed on start
            // and if they've become finished during game time, we shouldn't reschedule them
        }

        #endregion

        #region Callbacks

        private void OnLiveOpStateChanged(LiveOp liveOp)
        {
            LiveOpWrapper wrapper = liveOps.Find(x => x.LiveOp == liveOp);
            if (wrapper != null)
            {
                Schedule(wrapper);
            }

            save.Invoke();
            liveOpStateChanged.Invoke(liveOp);
        }

        private void OnLiveOpDataChanged(LiveOp liveOp)
        {
            LiveOpWrapper wrapper = liveOps.Find(x => x.LiveOp == liveOp);
            if (wrapper != null && wrapper.Progress.iFace.HasCompleted(wrapper.Progress.instance))
            {
                Schedule(wrapper);
            }

            save.Invoke();
        }

        #endregion
    }
}
