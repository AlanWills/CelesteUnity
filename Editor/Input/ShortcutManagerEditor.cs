using Celeste.Input;
using Celeste.Parameters.Input;
using CelesteEditor.Tools;
using System.Collections;
using System.Collections.Generic;
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
                AssetUtility.FindAssets<Shortcut>(target, "shortcuts");
            }

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
