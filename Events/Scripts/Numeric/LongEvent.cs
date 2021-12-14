using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class LongUnityEvent : UnityEvent<long> { }

    [CreateAssetMenu(fileName = nameof(LongEvent), menuName = "Celeste/Events/Long Event")]
    public class LongEvent : ParameterisedEvent<long>
    {
    }
}
