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
    [CustomEditor(typeof(TransformValue))]
    public class TransformEditor : ParameterValueEditor<Transform>
    {
        #region GUI

        protected override void DrawRuntimeInspector()
        {
            Parameter.Value = EditorGUILayout.ObjectField("Value", Parameter.Value, typeof(Transform), false) as Transform;
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
