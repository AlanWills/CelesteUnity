using Celeste.Debug.Menus;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Application.Debug
{
    [CreateAssetMenu(
        fileName = nameof(ApplicationDebugMenu), 
        menuName = CelesteMenuItemConstants.APPLICATION_MENU_ITEM + "Debug/Application Debug Menu",
        order = CelesteMenuItemConstants.APPLICATION_MENU_ITEM_PRIORITY)]
    public class ApplicationDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private BoolValue isEditor;
        [SerializeField] private BoolValue isMobile;
        [SerializeField] private BoolValue isDebug;

        #endregion

        protected override void OnDrawMenu()
        {
            GUILayout.Label($"Version: {UnityEngine.Application.version}");
            GUILayout.Label($"Screen Resolution: {Screen.width} x {Screen.height}");
            GUILayout.Label($"Screen Safe Area: {Screen.safeArea}");

            isEditor.Value = GUILayout.Toggle(isEditor.Value, "Is Editor");
            isMobile.Value = GUILayout.Toggle(isMobile.Value, "Is Mobile");
            isDebug.Value = GUILayout.Toggle(isDebug.Value, "Is Debug");
        }
    }
}
