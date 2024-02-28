using Celeste.Objects;
using UnityEngine;

namespace Celeste.Debug.Menus
{
    [CreateAssetMenu(
        fileName = nameof(DebugMenus), 
        menuName = CelesteMenuItemConstants.DEBUG_MENU_ITEM + "Debug Menus", 
        order = CelesteMenuItemConstants.DEBUG_MENU_ITEM_PRIORITY)]
    public class DebugMenus : ListScriptableObject<DebugMenu>
    {
        #region Properties and Fields

        public int MenuPriority
        {
            get { return menuPriority; }
            set
            {
                if (menuPriority != value)
                {
                    menuPriority = value;

                    Synchronize();
                }
            }
        }

        [SerializeField] private int menuPriority = 0;

        #endregion

        public void Synchronize()
        {
            for (int i = 0; i < NumItems; ++i)
            {
                DebugMenu debugMenu = GetItem(i);
                if (debugMenu != null)
                {
                    debugMenu.MenuPriority = i + menuPriority;
                }
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}
