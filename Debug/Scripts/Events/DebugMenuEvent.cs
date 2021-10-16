using Celeste.Debug.Menus;
using Celeste.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Debug.Events
{
    [Serializable]
    public class DebugMenuUnityEvent : UnityEvent<DebugMenu> { }

    [CreateAssetMenu(fileName = nameof(DebugMenuEvent), menuName = "Celeste/Events/Debug/Debug Menu Event")]
    public class DebugMenuEvent : ParameterisedEvent<DebugMenu> { }
}