using Celeste.Parameters;
using UnityEditor;

namespace CelesteEditor.Parameters.Bool
{
    [CustomEditor(typeof(BoolValue))]
    public class BoolValueEditor : ParameterValueEditor<BoolValue>
    {
        #region GUI

        protected override void DrawRuntimeInspector()
        {
            Parameter.Value = EditorGUILayout.Toggle("Value", Parameter.Value);
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
