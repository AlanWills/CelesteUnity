using Celeste.Debug.Menus;
using UnityEngine;

namespace Celeste.Application.Debug
{
    [CreateAssetMenu(
        fileName = nameof(ApplicationDebugMenu), 
        menuName = CelesteMenuItemConstants.APPLICATION_MENU_ITEM + "Debug/Application Debug Menu",
        order = CelesteMenuItemConstants.APPLICATION_MENU_ITEM_PRIORITY)]
    public class ApplicationDebugMenu : DebugMenu
    {
        protected override void OnDrawMenu()
        {
            GUILayout.Label($"Version: {UnityEngine.Application.version}");
            GUILayout.Label($"Screen Resolution: {Screen.width} x {Screen.height}");
            GUILayout.Label($"Screen Safe Area: {Screen.safeArea}");
        }
    }
}
