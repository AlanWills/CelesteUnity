using Celeste.Debug.Menus;
using Celeste.Events;
using UnityEngine;

namespace Celeste.Debug.Events
{
    [AddComponentMenu("Celeste/Debug/Events/Debug Menu Event Raiser")]
    public class DebugMenuEventRaiser : ParameterisedEventRaiser<DebugMenu, DebugMenuEvent>
    {
    }
}
