using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class KeyCodeUnityEvent : UnityEvent<KeyCode> { }

    [CreateAssetMenu(fileName = nameof(KeyCode), menuName = "Celeste/Events/Input/KeyCode Event")]
    public class KeyCodeEvent : ParameterisedEvent<KeyCode>
    {
    }
}
