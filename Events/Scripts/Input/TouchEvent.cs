using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class TouchUnityEvent : UnityEvent<Touch> { }

    [Serializable]
    [CreateAssetMenu(fileName = "TouchEvent", menuName = "Celeste/Events/Input/Touch Event")]
    public class TouchEvent : ParameterisedEvent<Touch>
    {
    }
}
