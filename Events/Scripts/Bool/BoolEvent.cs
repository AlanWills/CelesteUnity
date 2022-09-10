using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class BoolUnityEvent : UnityEvent<bool> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(BoolEvent), menuName = "Celeste/Events/Bool/Bool Event")]
    public class BoolEvent : ParameterisedEvent<bool>
    {
    }
}
