using Celeste.Components;
using System;
using UnityEngine;

namespace Celeste.LiveOps.Components
{
    [CreateAssetMenu(fileName = nameof(FixedDurationTimer), menuName = "Celeste/Live Ops/Timers/Fixed Duration")]
    public class FixedDurationTimer : Celeste.Components.Component, ILiveOpTimer
    {
        #region Save Data

        [Serializable]
        public class FixedDurationTimerSaveData : ComponentData
        {
            [HideInInspector] public long StartTime;
            public long Duration;
        }

        #endregion

        public override ComponentData CreateData()
        {
            return new FixedDurationTimerSaveData();
        }
        
        public void SetStartTimestamp(Instance instance, long startTimestamp)
        {
            FixedDurationTimerSaveData saveData = instance.data as FixedDurationTimerSaveData;
            saveData.StartTime = startTimestamp;
        }

        public long GetEndTimestamp(Instance instance)
        {
            FixedDurationTimerSaveData saveData = instance.data as FixedDurationTimerSaveData;
            return saveData.StartTime + saveData.Duration;
        }
    }
}
