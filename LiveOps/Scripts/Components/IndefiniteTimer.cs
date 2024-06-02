using Celeste.Components;
using UnityEngine;

namespace Celeste.LiveOps.Components
{
    [CreateAssetMenu(fileName = nameof(IndefiniteTimer), menuName = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM + "Timers/Indefinite Timer", order = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM_PRIORITY)]
    public class IndefiniteTimer : Celeste.Components.BaseComponent, ILiveOpTimer
    {
        public long GetEndTimestamp(Instance instance, long startTimestamp) { return long.MaxValue; }
    }
}
