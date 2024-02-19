using Celeste.Parameters;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Parameters
{
    [CustomEditor(typeof(CameraValue))]
    public class CameraValueEditor : ParameterValueEditor<CameraValue>
    {
        #region GUI

        protected override void DrawRuntimeInspector()
        {
            Parameter.Value = EditorGUILayout.ObjectField("Value", Parameter.Value, typeof(Camera), true) as Camera;
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
