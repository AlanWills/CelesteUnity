using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class FloatUnityEvent : UnityEvent<float> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(FloatEvent), menuName = "Celeste/Events/Numeric/Float Event")]
    public class FloatEvent : ParameterisedEvent<float>
    {
    }
}
