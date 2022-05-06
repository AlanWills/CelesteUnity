using Celeste.Debug.Menus;
using UnityEngine;

namespace Celeste.Application.Debug
{
    [CreateAssetMenu(fileName = nameof(ApplicationDebugMenu), menuName = "Celeste/Application/Debug/Application Debug Menu")]
    public class ApplicationDebugMenu : DebugMenu
    {
        protected override void OnDrawMenu()
        {
            GUILayout.Label($"Version: {UnityEngine.Application.version}");
        }
    }
}
