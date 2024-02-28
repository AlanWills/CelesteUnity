using Celeste.Debug.Menus;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Assets.Debug
{
    [CreateAssetMenu(
        fileName = nameof(AssetsDebugMenu), 
        menuName = CelesteMenuItemConstants.ASSETS_MENU_ITEM + "Debug/Assets Debug Menu",
        order = CelesteMenuItemConstants.ASSETS_MENU_ITEM_PRIORITY)]
    public class AssetsDebugMenu : DebugMenu
    {
        protected override void OnDrawMenu()
        {
            if (GUILayout.Button("Clear Cache"))
            {
                CachingUtility.ClearCache();
            }
        }
    }
}
