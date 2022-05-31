using Celeste.LiveOps;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Live Ops/Live Op Event Raiser")]
    public class LiveOpEventRaiser : ParameterisedEventRaiser<LiveOp, LiveOpEvent>
    {
    }
}
