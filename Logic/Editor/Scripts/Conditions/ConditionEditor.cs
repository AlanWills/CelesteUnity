using Celeste.Logic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Logic
{
    [CustomEditor(typeof(Condition), true, isFallback = true)]
    public class ConditionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
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
