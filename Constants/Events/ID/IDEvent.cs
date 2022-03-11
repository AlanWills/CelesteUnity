using Celeste.Constants;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class IDUnityEvent : UnityEvent<ID> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(ID), menuName = "Celeste/Events/Constants/ID Event")]
    public class IDEvent : ParameterisedEvent<ID>
    {
    }
}
