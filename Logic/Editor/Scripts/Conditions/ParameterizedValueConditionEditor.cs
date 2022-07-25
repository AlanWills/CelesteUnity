using Celeste.Logic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace CelesteEditor.Logic
{
    public abstract class ParameterizedValueConditionEditor : ConditionEditor
    {
        protected void DrawDefaultInspectorGUI(SerializedObject valueCondition, string[] operatorDisplayNames, int[] operators)
        {
            using (VerticalScope vertical = new VerticalScope())
            {
                SerializedProperty valueProperty = valueCondition.FindProperty("value");
                SerializedProperty conditionProperty = valueCondition.FindProperty("condition");
                SerializedProperty targetProperty = valueCondition.FindProperty("target");

                EditorGUILayout.PropertyField(valueProperty);

                int chosenOperator = conditionProperty.enumValueIndex;
                chosenOperator = EditorGUILayout.IntPopup(chosenOperator, operatorDisplayNames, operators);
                conditionProperty.enumValueIndex = chosenOperator;

                EditorGUILayout.PropertyField(targetProperty);
            }
        }
    }
}
