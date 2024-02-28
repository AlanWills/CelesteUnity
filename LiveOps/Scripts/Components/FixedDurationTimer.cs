using Celeste.Components;
using System;
using UnityEngine;

namespace Celeste.LiveOps.Components
{
    [CreateAssetMenu(fileName = nameof(FixedDurationTimer), menuName = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM + "Timers/Fixed Duration", order = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM_PRIORITY)]
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
