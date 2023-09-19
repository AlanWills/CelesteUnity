﻿using Celeste.Assets;
using Celeste.Core;
using Celeste.Log;
using Celeste.Persistence;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.LiveOps.Persistence
{
    [AddComponentMenu("Celeste/Live Ops/Live Ops Manager")]
    public class LiveOpsManager : PersistentSceneManager<LiveOpsManager, LiveOpsDTO>, IHasAssets
    {
        #region Properties and Fields

        public const string FILE_NAME = "LiveOps.dat";
        protected override string FileName => FILE_NAME;

        // This is fallback data for the client to use, in case we aren't able to obtain anything from the game server
        [SerializeField] private LiveOpsSchedule liveOpsSchedule;
        [SerializeField] private LiveOpsRecord liveOpsRecord;

        #endregion

        #region Save/Load

        protected override LiveOpsDTO Serialize()
        {
            return new LiveOpsDTO(liveOpsRecord);
        }

        protected override void Deserialize(LiveOpsDTO dto)
        {
            StartCoroutine(AddAllLiveOps(dto));
        }

        protected override void SetDefaultValues()
        {
            StartCoroutine(AddLiveOpsFromSchedule());
        }

        #endregion

        #region IHasAssets

        public bool ShouldLoadAssets()
        {
            return true;
        }

        public IEnumerator LoadAssets()
        {
            Load();

            yield break;
        }

        #endregion

        private IEnumerator AddAllLiveOps(LiveOpsDTO liveOpsDTO)
        {
            // Add all our live ops from the save data
            foreach (LiveOpDTO liveOpDTO in liveOpsDTO.liveOps)
            {
                if (liveOpDTO != null && liveOpDTO.IsValid)
                {
                    yield return liveOpsRecord.AddLiveOp(liveOpDTO);
                }
                else
                {
                    UnityEngine.Debug.Assert(liveOpDTO != null, $"Null liveop coming from save - probably a deserialization issue.");
                    HudLog.LogWarning($"Skipping invalid live op dto from save.");
                }
            }

            yield return AddLiveOpsFromSchedule();
        }

        private IEnumerator AddLiveOpsFromSchedule()
        {
            // Add any missing liveops from our schedule too, but make sure they're starting now if they will be added
            foreach (LiveOpDTO liveOpDTO in liveOpsSchedule.Items)
            {
                if (liveOpDTO != null && liveOpDTO.IsValid)
                {
                    yield return liveOpsRecord.AddLiveOp(liveOpDTO);
                }
                else
                {
                    UnityEngine.Debug.Assert(liveOpDTO != null, $"Null liveop coming from schedule - probably a bug in the pipeline or out of date data.");
                    HudLog.LogWarning($"Skipping invalid live op dto from schedule.");
                }
            }
        }

        #region Callbacks

        public void OnLiveOpStateChanged(LiveOp liveOp)
        {
            // Calculate the latest possible start timestamp in the past based on the liveop start timestamp and the recurrence frequency
            if (liveOp.State == LiveOpState.Finished && liveOp.IsRecurring && liveOp.RepeatsAfter > 0)
            {
                long diffBetweenNowAndStart = GameTime.UtcNowTimestamp - liveOp.StartTimestamp;
                long liveOpStartTimestamp = 0;

                if (diffBetweenNowAndStart < liveOp.RepeatsAfter)
                {
                    liveOpStartTimestamp = liveOp.StartTimestamp + liveOp.RepeatsAfter;
                }
                else
                {
                    liveOpStartTimestamp = GameTime.UtcNowTimestamp - (diffBetweenNowAndStart % liveOp.RepeatsAfter);
                }

                DateTimeOffset debugLiveOpStartTimestamp = GameTime.ToDateTimeOffset(liveOpStartTimestamp);
                UnityEngine.Debug.Log($"Recurring liveop scheduled for {debugLiveOpStartTimestamp}");

                StartCoroutine(liveOpsRecord.AddLiveOp(liveOp, liveOpStartTimestamp));
            }
        }

        #endregion
    }
}
