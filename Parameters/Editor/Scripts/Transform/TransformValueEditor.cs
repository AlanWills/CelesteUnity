using Celeste.Parameters;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Parameters
{
    [CustomEditor(typeof(TransformValue))]
    public class TransformEditor : ParameterValueEditor<TransformValue>
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
