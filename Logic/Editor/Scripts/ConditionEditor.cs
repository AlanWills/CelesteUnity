using Celeste.Logic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace CelesteEditor.Logic
{
    public abstract class ConditionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedObject serializedObject = new SerializedObject(target);
            serializedObject.Update();

            OnInspectorGUIImpl(serializedObject);

            serializedObject.ApplyModifiedProperties();
        }

        protected abstract void OnInspectorGUIImpl(SerializedObject eventConditionObject);

        protected void DrawDefaultInspectorGUI(SerializedObject valueCondition, string[] operatorDisplayNames, int[] operators)
        {
            using (VerticalScope vertical = new VerticalScope())
            {
                SerializedProperty valueProperty = valueCondition.FindProperty("value");
                SerializedProperty conditionProperty = valueCondition.FindProperty("condition");
                SerializedProperty targetProperty = valueCondition.FindProperty("target");

                if (targetProperty.objectReferenceValue == null)
                {
                    if (GUILayout.Button("Init"))
                    {
                        (valueCondition.targetObject as Condition).Init();
                    }

                    return;
                }

                EditorGUILayout.PropertyField(valueProperty);

                int chosenOperator = conditionProperty.enumValueIndex;
                chosenOperator = EditorGUILayout.IntPopup(chosenOperator, operatorDisplayNames, operators);
                conditionProperty.enumValueIndex = chosenOperator;

                EditorGUILayout.PropertyField(targetProperty);
            }
        }
    }
}
