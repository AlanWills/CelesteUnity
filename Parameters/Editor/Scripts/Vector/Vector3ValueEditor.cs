using Celeste.Parameters;
using UnityEditor;

namespace CelesteEditor.Parameters.Vector
{
    [CustomEditor(typeof(Vector3Value))]
    public class Vector3Editor : ParameterValueEditor<Vector3Value>
    {
        #region GUI

        protected override void DrawRuntimeInspector()
        {
            Parameter.Value = EditorGUILayout.Vector3Field("Value", Parameter.Value);
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
