using Celeste.LiveOps;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class LiveOpUnityEvent : UnityEvent<LiveOp> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(LiveOpEvent), menuName = "Celeste/Events/Live Ops/Live Op Event")]
    public class LiveOpEvent : ParameterisedEvent<LiveOp>
    {
    }
}
