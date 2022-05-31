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
        public class FixedDurationTimerData : ComponentData
        {
            [HideInInspector] public long StartTime;
            public long Duration;
        }

        #endregion

        public override ComponentData CreateData()
        {
            return new FixedDurationTimerData();
        }
        
        public void SetStartTimestamp(Instance instance, long startTimestamp)
        {
            FixedDurationTimerData saveData = instance.data as FixedDurationTimerData;
            saveData.StartTime = startTimestamp;
        }

        public long GetEndTimestamp(Instance instance)
        {
            FixedDurationTimerData saveData = instance.data as FixedDurationTimerData;
            return saveData.StartTime + saveData.Duration;
        }
    }
}
