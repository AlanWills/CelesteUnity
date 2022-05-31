using Celeste.Components;
using UnityEngine;

namespace Celeste.LiveOps.Components
{
    [CreateAssetMenu(fileName = nameof(IndefiniteTimer), menuName = "Celeste/Live Ops/Timers/Indefinite Timer")]
    public class IndefiniteTimer : Celeste.Components.Component, ILiveOpTimer
    {
        public long GetEndTimestamp(Instance instance, long startTimestamp) { return long.MaxValue; }
    }
}
