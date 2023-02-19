using Celeste.Debug.Menus;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace CelesteEditor.Debug
{
    [CustomEditor(typeof(CompoundDebugMenu))]
    public class CompoundDebugMenuEditor : Editor
    {
        #region Properties and Fields

        private SerializedProperty debugMenusProperty;

        #endregion

        private void OnEnable()
        {
            debugMenusProperty = serializedObject.FindProperty("debugMenus");
        }

        public override void OnInspectorGUI()
        {
            CompoundDebugMenu debugMenu = target as CompoundDebugMenu;

            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script", "debugMenus");

            EditorGUILayout.Space();

            if (GUILayout.Button("Synchronize"))
            {
                debugMenu.Synchronize();
            }

            using (var changeCheck = new ChangeCheckScope())
            {
                EditorGUILayout.PropertyField(debugMenusProperty);

                if (changeCheck.changed)
                {
                    debugMenu.Synchronize();
                }
            }

            debugMenu.DrawMenu();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
