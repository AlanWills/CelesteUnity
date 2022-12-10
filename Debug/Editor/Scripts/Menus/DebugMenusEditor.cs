using Celeste.Debug.Menus;
using CelesteEditor.DataStructures;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Debug.Menus
{
    [CustomEditor(typeof(DebugMenus))]
    public class DebugMenusEditor : IIndexableItemsEditor<DebugMenu>
    {
        public override void OnInspectorGUI()
        {
            DebugMenus debugMenus = target as DebugMenus;

            if (GUILayout.Button("Synchronize"))
            {
                debugMenus.Synchronize();
            }

            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                base.OnInspectorGUI();

                if (changeCheck.changed)
                {
                    debugMenus.Synchronize();
                }
            }
        }
    }
}
