using Celeste.Parameters;
using UnityEditor;

namespace CelesteEditor.Parameters.Vector
{
    [CustomEditor(typeof(Vector3IntValue))]
    public class Vector3IntEditor : ParameterValueEditor<Vector3IntValue>
    {
        #region GUI

        protected override void DrawRuntimeInspector()
        {
            Parameter.Value = EditorGUILayout.Vector3IntField("Value", Parameter.Value);
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
