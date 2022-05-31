using Celeste.LiveOps;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Live Ops/Live Op Event Listener")]
    public class LiveOpEventListener : ParameterisedEventListener<LiveOp, LiveOpEvent, LiveOpUnityEvent>
    {
    }
}
