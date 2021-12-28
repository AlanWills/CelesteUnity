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

        private void Start()
        {
            registerDebugMenu.InvokeSilently(debugMenu);
        }

        private void OnDestroy()
        {
            deregisterDebugMenu.InvokeSilently(debugMenu);
        }
    }
}