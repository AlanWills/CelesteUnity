using Celeste.Debug.Events;
using System.Collections;
using UnityEngine;

namespace Celeste.Debug.Menus
{
    [AddComponentMenu("Celeste/Debug/Menus/Register Debug Menu")]
    public class RegisterDebugMenu : MonoBehaviour
    {
        [SerializeField] private DebugMenu debugMenu;
        [SerializeField] private DebugMenuEvent registerDebugMenu;
        [SerializeField] private DebugMenuEvent deregisterDebugMenu;

        private void OnEnable()
        {
            UnityEngine.Debug.Assert(debugMenu != null, $"No {nameof(debugMenu)} assigned in {nameof(RegisterDebugMenu)} on object {name}.", this);
            if (debugMenu != null)
            {
                UnityEngine.Debug.Assert(registerDebugMenu != null, $"No {nameof(registerDebugMenu)} variable assigned in {nameof(RegisterDebugMenu)} on object {name}.", this);
                registerDebugMenu?.InvokeSilently(debugMenu);
            }
        }

        private void OnDisable()
        {
            if (debugMenu != null)
            {
                deregisterDebugMenu?.InvokeSilently(debugMenu);
            }
        }
    }
}