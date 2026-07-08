using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Debug.Menus
{
    [CreateAssetMenu(fileName = nameof(DebugSettingsDebugMenu), menuName = CelesteMenuItemConstants.DEBUG_MENU_ITEM + "Debug Settings Debug Menu", order = CelesteMenuItemConstants.DEBUG_MENU_ITEM_PRIORITY)]
    public class DebugSettingsDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private BoolValue isDeveloperConsoleEnabled;

        #endregion

        protected override void OnDrawMenu()
        {
            base.OnDrawMenu();

            isDeveloperConsoleEnabled.Value =
                GUILayout.Toggle(isDeveloperConsoleEnabled.Value, "Is Developer Console Enabled");
        }
    }
}