﻿using Celeste.Debug.Menus;
using Celeste.Parameters;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Celeste.Scene.Debug
{
    [CreateAssetMenu(fileName = nameof(SceneDebugMenu), order = CelesteMenuItemConstants.SCENE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SCENE_MENU_ITEM + "Debug/Scene Debug Menu")]
    public class SceneDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private BoolValue isRuntimeHierarchyVisible;
        [SerializeField] private BoolValue isRuntimeInspectorVisible;

        #endregion

        #region GUI

        protected override void OnDrawMenu()
        {
            isRuntimeHierarchyVisible.Value = Toggle(isRuntimeHierarchyVisible.Value, "Runtime Hierarchy Visible");
            isRuntimeInspectorVisible.Value = Toggle(isRuntimeInspectorVisible.Value, "Runtime Inspector Visible");
        }

        #endregion
    }
}