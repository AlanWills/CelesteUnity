using Celeste;
using CelesteEditor.Tools;
using System;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor
{
    public partial class CelesteEditorGUILayout
    {
        public static int SubAssetListField(
            string title,
            Type[] types,
            string[] displayNames,
            int selectedTypeIndex,
            SerializedProperty listProperty,
            Action<Type> onAdd,
            Action<int> onRemove)
        {
            EditorGUILayout.LabelField(title, CelesteGUIStyles.BoldLabel);

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            selectedTypeIndex = EditorGUILayout.Popup("Type", selectedTypeIndex, displayNames);
            if (GUILayout.Button("Add", GUILayout.ExpandWidth(false)))
            {
                onAdd(types[selectedTypeIndex]);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
            ++EditorGUI.indentLevel;

            for (int i = listProperty.arraySize; i > 0; --i)
            {
                UnityEngine.Object obj = listProperty.GetArrayElementAtIndex(i - 1).objectReferenceValue;

                using (EditorGUILayout.HorizontalScope horizontal = new EditorGUILayout.HorizontalScope())
                {
                    string label = obj != null ? $"{obj.GetType().GetDisplayName()} ({obj.name})" : "NULL";
                    EditorGUILayout.LabelField(label, CelesteGUIStyles.BoldLabel);

                    if (GUILayout.Button("-", GUILayout.ExpandWidth(false)))
                    {
                        onRemove(i - 1);
                    }
                }

                EditorGUILayout.Space();

                if (obj != null)
                {
                    Editor editor = Editor.CreateEditor(obj);
                    editor.OnInspectorGUI();
                }

                EditorGUILayout.Space();
                CelesteEditorGUILayout.HorizontalLine();
            }

            --EditorGUI.indentLevel;
            EditorGUILayout.Space();

            return selectedTypeIndex;
        }
    }
}
