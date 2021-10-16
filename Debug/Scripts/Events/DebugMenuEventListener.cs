using Celeste.Debug.Menus;
using Celeste.Events;
using UnityEngine;

namespace Celeste.Debug.Events
{
    [AddComponentMenu("Celeste/Debug/Events/Debug Menu Event Listener")]
    public class DebugMenuEventListener : ParameterisedEventListener<DebugMenu, DebugMenuEvent, DebugMenuUnityEvent>
    {
    }
}
