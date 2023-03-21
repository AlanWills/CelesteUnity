using Celeste.Logic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace CelesteEditor.Logic
{
    [CustomEditor(typeof(Condition), true, isFallback = true)]
    public class ConditionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedObject serializedObject = new SerializedObject(target);
            serializedObject.Update();

            if (GUILayout.Button("Initialize"))
            {
                (target as Condition).Initialize();
            }

            OnInspectorGUIImpl(serializedObject);

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnInspectorGUIImpl(SerializedObject eventConditionObject)
        {
            DrawPropertiesExcluding(eventConditionObject, "m_Script");
        }
    }
}
