using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Debug.Menus
{
    [CreateAssetMenu(
        fileName = nameof(CompoundDebugMenu), 
        menuName = CelesteMenuItemConstants.DEBUG_MENU_ITEM + "Compound Debug Menu",
        order = CelesteMenuItemConstants.DEBUG_MENU_ITEM_PRIORITY)]
    public class CompoundDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private List<DebugMenu> debugMenus = new List<DebugMenu>();

        private int currentDebugMenu = -1;

        #endregion

        protected override void OnDrawMenu()
        {
            if (currentDebugMenu < 0)
            {
                List<DebugMenu> sortedDebugMenus = new List<DebugMenu>(debugMenus);
                sortedDebugMenus.Sort((a, b) =>
                {
                    int menuPriority = b.MenuPriority - a.MenuPriority;

                    if (menuPriority != 0)
                    {
                        return menuPriority;
                    }

                    return string.CompareOrdinal(a.DisplayName, b.DisplayName);
                });

                for (int i = 0, n = debugMenus.Count; i < n; i++)
                {
                    var debugMenu = debugMenus[i];

                    if (debugMenu != null)
                    {
                        if (GUILayout.Button(debugMenu.DisplayName))
                        {
                            currentDebugMenu = i;
                            debugMenu.Visible = true;
                        }
                    }
                }
            }
            else
            {
                if (GUILayout.Button("Close"))
                {
                    debugMenus[currentDebugMenu].Visible = false;
                    currentDebugMenu = -1;
                }

                if (currentDebugMenu >= 0)
                {
                    debugMenus[currentDebugMenu].DrawMenu();
                }
            }
        }

        protected override void OnHideMenu()
        {
            base.OnHideMenu();

            // Make sure the current debug menu is closed when we close this one
            currentDebugMenu = -1;
        }

        public void Synchronize()
        {
            for (int i = 0, n = debugMenus.Count; i < n; ++i)
            {
                if (debugMenus[i] != null)
                {
                    debugMenus[i].MenuPriority = i;
                }
            }

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}