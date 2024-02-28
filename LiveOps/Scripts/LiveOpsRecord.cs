﻿using Celeste.Components;
using Celeste.Components.Catalogue;
using Celeste.Components.Persistence;
using Celeste.Core;
using Celeste.DataStructures;
using Celeste.Events;
using Celeste.LiveOps.Persistence;
using Celeste.Rewards.Catalogue;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.LiveOps
{
    [CreateAssetMenu(fileName = nameof(LiveOpsRecord), menuName = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM + "Live Ops Record", order = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM_PRIORITY)]
    public class LiveOpsRecord : ScriptableObject
    {
        #region Properties and Fields

        public int NumLiveOps => liveOps.Count;

        [Header("Dependencies")]
        [SerializeField] private ComponentCatalogue liveOpsComponentCatalogue;
        [SerializeField] private ScheduledCallbacks scheduledCallbacks;
        [SerializeField] private RewardCatalogue rewardCatalogue;

        [Header("Events")]
        [SerializeField] private LiveOpEvent liveOpAdded;
        [SerializeField] private LiveOpEvent liveOpStateChanged;
        [SerializeField] private Events.Event save;

        [NonSerialized] private List<LiveOp> liveOps = new List<LiveOp>();
        [NonSerialized] private List<ValueTuple<LiveOp, CallbackHandle>> scheduleCallbackHandles = new List<ValueTuple<LiveOp, CallbackHandle>>();

        #endregion

        public LiveOp GetLiveOp(int index)
        {
            return liveOps.Get(index);
        }

        public void RemoveLiveOp(int index)
        {
            LiveOp liveOp = liveOps.Get(index);
            liveOps.RemoveAt(index);
            HandleScheduleOfFinishedLiveOp(liveOp);
            save.Invoke();
        }

        public void RemoveAllLiveOps()
        {
            for (int i = liveOps.Count - 1; i >= 0; --i)
            {
                RemoveLiveOp(i);
            }
        }

        public IEnumerator AddLiveOp(LiveOp liveOp, long startTimestamp)
        {
            return AddLiveOp(new LiveOpDTO(liveOp), startTimestamp);
        }

        public IEnumerator AddLiveOp(LiveOpDTO liveOpDTO)
        {
            return AddLiveOp(liveOpDTO, liveOpDTO.startTimestamp);
        }

        public IEnumerator AddLiveOp(LiveOpDTO liveOpDTO, long startTimestamp)
        {
            long liveOpType = liveOpDTO.type;
            long liveOpSubType = liveOpDTO.subType;
            long liveOpStartTimestamp = startTimestamp;
            LiveOpState liveOpState = liveOpDTO.state;

            if (liveOpState == LiveOpState.Unknown)
            {
                UnityEngine.Debug.LogAssertion($"Unknown liveop state found.  This is a serious error, so the liveop will probably not be scheduled...");
                yield break;
            }

            if (liveOps.Exists(x =>
                x.Type == liveOpType &&
                x.SubType == liveOpSubType &&
                x.StartTimestamp == startTimestamp))
            {
                UnityEngine.Debug.Log($"Live Op with type {liveOpDTO.type} and subtype {liveOpDTO.subType} starting at timestamp {liveOpDTO.startTimestamp} is already added to the liveops record and will not be re-added.");
                yield break;
            }

            if (liveOpDTO.isRecurring)
            {
                for (int i = 0, n = NumLiveOps; i < n; ++i)
                {
                    LiveOp lo = liveOps[i];

                    if (lo.Type == liveOpDTO.type && lo.SubType == liveOpDTO.subType)
                    {
                        UnityEngine.Debug.Log($"Recurring Live Op with type {liveOpDTO.type} and subtype {liveOpDTO.subType} already has a matching liveop added to the liveops record and so will not be added.  Only one instance of a recurring liveop can exist in the record.");
                        yield break;
                    }
                }
            }

            if (liveOpState == LiveOpState.Finished)
            {
                UnityEngine.Debug.Log($"Live Op with type {liveOpDTO.type} starting at timestamp {liveOpDTO.startTimestamp} has finished, so will not be added.");

                if (!liveOpDTO.isRecurring)
                {
                    // This live op is not recurring so we will not add it
                    yield break;
                }

                // Set the state to ComingSoon, so it'll be handled properly when we schedule - we can't do more without the timer
                liveOpState = LiveOpState.ComingSoon;
            }

            LiveOpComponents liveOpComponents = new LiveOpComponents();

            UnityEngine.Debug.Assert(liveOpDTO.IsValid, $"Trying to add an invalid live to the live op record.");
            foreach (ComponentDTO componentDTO in liveOpDTO.components)
            {
                var componentHandle = liveOpsComponentCatalogue.CreateComponent<Celeste.Components.Component>(componentDTO.typeName, componentDTO.data);
                if (componentHandle.IsValid)
                {
                    liveOpComponents.AddComponent(componentHandle);
                }
            }

            if (!liveOpComponents.TryFindComponent<ILiveOpTimer>(out var timer))
            {
                UnityEngine.Debug.LogAssertion($"Live Op with type {liveOpType} starting at timestamp {liveOpStartTimestamp} has no {nameof(ILiveOpTimer)} component, so will not be scheduled.");
                yield break;
            }

            if (!liveOpComponents.TryFindComponent<ILiveOpProgress>(out var progress))
            {
                UnityEngine.Debug.LogAssertion($"Live Op with type {liveOpType} starting at timestamp {liveOpStartTimestamp} has no {nameof(ILiveOpProgress)} component, so will not be scheduled.");
                yield break;
            }

            if (!liveOpComponents.TryFindComponent<ILiveOpAssets>(out var assets))
            {
                UnityEngine.Debug.LogAssertion($"Live Op with type {liveOpType} starting at timestamp {liveOpStartTimestamp} has no {nameof(ILiveOpAssets)} component, so will not be scheduled.");
                yield break;
            }

            LiveOp liveOp = new LiveOp(
                liveOpType,
                liveOpSubType,
                liveOpStartTimestamp,
                liveOpDTO.isRecurring,
                liveOpDTO.repeatsAfter,
                liveOpState,
                liveOpComponents, 
                timer, 
                progress, 
                assets);

            yield return liveOp.Load();
            
            if (liveOp.CanSchedule)
            {
                liveOps.Add(liveOp);

                SetUpLiveOpCallbacks(liveOp);
                Schedule(liveOp);

                liveOpAdded.Invoke(liveOp);
            }
        }

        public bool TryFindNextScheduledLiveOp<T>(out LiveOp nextScheduledLiveOp) where T : class
        {
            LiveOp earliestDailyTasksLiveOp = null;

            for (int i = 0, n = NumLiveOps; i < n; ++i)
            {
                LiveOp liveOp = GetLiveOp(i);

                if (liveOp.HasComponent<T>() && liveOp.StartTimestamp > GameTime.UtcNowTimestamp)
                {
                    if (earliestDailyTasksLiveOp == null || earliestDailyTasksLiveOp.StartTimestamp > liveOp.StartTimestamp)
                    {
                        earliestDailyTasksLiveOp = liveOp;
                    }
                }
            }

            nextScheduledLiveOp = earliestDailyTasksLiveOp;
            return nextScheduledLiveOp != null;
        }

        #region Scheduling

        private void Schedule(LiveOp liveOp)
        {
            int callbackIndex = scheduleCallbackHandles.FindIndex(x => x.Item1 == liveOp);
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

        private void HandleScheduleOfComingSoonLiveOp(LiveOp liveOp)
        {
            if (liveOp.StartTimestamp <= GameTime.UtcNowTimestamp)
            {
                if (liveOp.EndTimestamp > GameTime.UtcNowTimestamp)
                {
                    // We can actually start the live op now, so let's do it!
                    liveOp.Start();
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

        private void HandleScheduleOfRunningLiveOp(LiveOp liveOp)
        {
            long endTime = liveOp.EndTimestamp;

            if (endTime <= GameTime.UtcNowTimestamp)
            {
                // Event has timed out - use the progress interface to see if we can just dismiss the event
                if (!liveOp.PlayerActionRequired)
                {
                    // We can immediately end this event now as the player has no unresolved progress
                    liveOp.Finish();
                }
            }
            else
            {
                // Event still running, but we could have completed it so we check here
                if (liveOp.ProgressRatio >= 1f)
                {
                    // We actually have completed it!
                    liveOp.Complete();
                }
                else
                {
                    // Schedule a callback to handle the timeout of the event
                    CallbackHandle callbackHandle = scheduledCallbacks.Schedule(endTime, () => Schedule(liveOp));
                    scheduleCallbackHandles.Add((liveOp, callbackHandle));
                }
            }
        }

        private void HandleScheduleOfCompletedLiveOp(LiveOp liveOp)
        {
            long endTime = liveOp.EndTimestamp;

            if (endTime <= GameTime.UtcNowTimestamp)
            {
                // Event has timed out.  Since we've completed it already, we can just finish it here
                liveOp.Finish();
            }
            else
            {
                // Schedule a callback to handle the timeout of the event
                CallbackHandle callbackHandle = scheduledCallbacks.Schedule(endTime, () => Schedule(liveOp));
                scheduleCallbackHandles.Add((liveOp, callbackHandle));
            }
        }

        private void HandleScheduleOfFinishedLiveOp(LiveOp liveOp)
        {
            TearDownLiveOpCallbacks(liveOp);
            liveOps.Remove(liveOp);
        }

        #endregion

        #region Callbacks

        private void SetUpLiveOpCallbacks(LiveOp liveOp)
        {
            liveOp.StateChanged.AddListener(OnLiveOpStateChanged);
            liveOp.ProgressChanged.AddListener(OnLiveOpProgressChanged);
            liveOp.DataChanged.AddListener(OnLiveOpDataChanged);
        }

        private void TearDownLiveOpCallbacks(LiveOp liveOp)
        {
            liveOp.StateChanged.RemoveListener(OnLiveOpStateChanged);
            liveOp.ProgressChanged.RemoveListener(OnLiveOpProgressChanged);
            liveOp.DataChanged.RemoveListener(OnLiveOpDataChanged);
        }

        private void OnLiveOpStateChanged(LiveOp liveOp)
        {
            Schedule(liveOp);

            save.Invoke();
            liveOpStateChanged.Invoke(liveOp);
        }

        private void OnLiveOpProgressChanged(LiveOp liveOp)
        {
            if (liveOp.State == LiveOpState.Running && liveOp.ProgressRatio >= 1)
            {
                // Our live op is running, but we have now completed it
                liveOp.Complete();
            }

            Schedule(liveOp);
            save.Invoke();
        }

        private void OnLiveOpDataChanged(LiveOp liveOp)
        {
            save.Invoke();
        }

        #endregion
    }
}
