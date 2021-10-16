using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Parameters
{
    public abstract class ParameterValueEditor<T> : Editor
    {
        #region Properties and Fields

        protected ParameterValue<T> Parameter
        {
            get { return target as ParameterValue<T>; }
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
