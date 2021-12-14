using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class FloatUnityEvent : UnityEvent<float> { }

    [Serializable]
    [CreateAssetMenu(fileName = "FloatEvent", menuName = "Celeste/Events/Float Event")]
    public class FloatEvent : ParameterisedEvent<float>
    {
    }
}
