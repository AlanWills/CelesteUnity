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
            public long Duration;
        }

        #endregion

        public override ComponentData CreateData()
        {
            return new FixedDurationTimerData();
        }
        
        public long GetEndTimestamp(Instance instance, long startTimestamp)
        {
            FixedDurationTimerData saveData = instance.data as FixedDurationTimerData;
            return startTimestamp + saveData.Duration;
        }
    }
}
