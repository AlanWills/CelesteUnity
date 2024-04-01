using Celeste.Input;
using Celeste.Parameters.Input;
using CelesteEditor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                EditorOnly.FindAssets<KeyCodeValue>(target, "keyCodes");
            }

            EditorGUILayout.Space();

            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
