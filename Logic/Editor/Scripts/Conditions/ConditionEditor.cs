using Celeste.Logic;
using Celeste.Objects;
using CelesteEditor.Objects;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Logic
{
    [CustomEditor(typeof(Condition), true, isFallback = true)]
    public class ConditionEditor : IEditorInitializableEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if ((target is IEditorInitializable) && GUILayout.Button("Initialize"))
            {
                (target as IEditorInitializable).Editor_Initialize();
            }

            Condition condition = target as Condition;

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField($"Is Condition Met?", condition.IsMet.ToString());

                if (GUILayout.Button("Check"))
                {
                    condition.Check();
                }
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
