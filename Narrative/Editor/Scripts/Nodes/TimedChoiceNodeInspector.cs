using Celeste;
using Celeste.Narrative;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace CelesteEditor.Narrative
{
    [CustomEditor(typeof(TimedChoiceNode))]
    public class TimedChoiceNodeInspector : Editor
    {
        private SerializedProperty choicesProperty;

        private void OnEnable()
        {
            choicesProperty = serializedObject.FindProperty("choices");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script", "choices");

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Choices", CelesteGUIStyles.BoldLabel);

            for (int i = 0, n = choicesProperty.arraySize; i < n; ++i)
            {
                EditorGUILayout.Space();

                var choice = choicesProperty.GetArrayElementAtIndex(i).objectReferenceValue;
                EditorGUILayout.LabelField(choice.name, CelesteGUIStyles.BoldLabel);

                using (IndentLevelScope indent = new IndentLevelScope())
                {
                    Editor choicesEditor = CreateEditor(choice);
                    choicesEditor.OnInspectorGUI();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}