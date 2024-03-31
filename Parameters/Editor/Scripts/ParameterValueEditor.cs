using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Parameters
{
    public abstract class ParameterValueEditor<T> : Editor where T : ScriptableObject
    {
        #region Properties and Fields

        protected T Parameter
        {
            get { return target as T; }
        }

        #endregion

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (Application.isPlaying)
            {
                DrawRuntimeInspector();
            }
            else
            {
                DrawEditorInspector();
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        protected virtual void DrawEditorInspector()
        {
            DrawPropertiesExcluding(serializedObject, "m_Script");
        }

        protected abstract void DrawRuntimeInspector();
    }
}
