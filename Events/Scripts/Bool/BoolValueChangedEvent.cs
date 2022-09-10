using System;
using UnityEngine;

namespace Celeste.Events
{
    [Serializable]
    public class BoolValueChangedUnityEvent : ValueChangedUnityEvent<bool> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(BoolValueChangedEvent), menuName = "Celeste/Events/Bool/Bool Value Changed Event")]
    public class BoolValueChangedEvent : ParameterisedValueChangedEvent<bool>
    {
    }
}
