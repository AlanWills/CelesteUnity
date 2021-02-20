using Celeste.FSM.Nodes.Logic;
using Celeste.FSM.Nodes.Logic.Conditions;
using CelesteEditor.PropertyDrawers.Parameters;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.FSM.Nodes.Logic.Conditions
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

        protected void DrawDefaultInspectorGUI(SerializedObject valueCondition, string[] operatorDisplayNames, int[] operators, Object conditionTarget)
        {
            EditorGUILayout.BeginVertical();

            SerializedProperty valueProperty = valueCondition.FindProperty("value");
            SerializedProperty conditionProperty = valueCondition.FindProperty("condition");
            SerializedProperty useArgumentProperty = valueCondition.FindProperty("useArgument");

            EditorGUILayout.PropertyField(valueProperty, GUIContent.none);

            int chosenOperator = conditionProperty.enumValueIndex;
            chosenOperator = EditorGUILayout.IntPopup(chosenOperator, operatorDisplayNames, operators);
            conditionProperty.enumValueIndex = chosenOperator;

            EditorGUILayout.PropertyField(useArgumentProperty);

            if (!useArgumentProperty.boolValue)
            {
                ParameterReferencePropertyDrawer.OnGUI(conditionTarget);
            }

            EditorGUILayout.EndVertical();
        }
    }
}
