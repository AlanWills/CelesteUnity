using Celeste.Narrative.Choices;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Narrative.Choices
{
    [CustomEditor(typeof(Choice))]
    public class ChoiceEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }
    }
}