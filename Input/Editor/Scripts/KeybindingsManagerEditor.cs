using Celeste.Input;
using Celeste.Parameters.Input;
using CelesteEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Input
{
    [CustomEditor(typeof(KeybindingsManager))]
    public class KeybindingsManagerEditor : Editor
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Find KeyCode Values", GUILayout.ExpandWidth(false)))
            {
                AssetDatabaseExtensions.FindAssets<KeyCodeValue>(target, "keyCodes");
            }

            EditorGUILayout.Space();

            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
