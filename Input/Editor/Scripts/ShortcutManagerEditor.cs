using Celeste.Input;
using CelesteEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Input
{
    [CustomEditor(typeof(ShortcutManager))]
    public class ShortcutManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("Find Shortcuts"))
            {
                AssetDatabaseExtensions.FindAssets<Shortcut>(target, "shortcuts");
            }

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
