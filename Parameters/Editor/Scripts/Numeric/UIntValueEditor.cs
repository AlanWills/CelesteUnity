using Celeste.Parameters;
using UnityEditor;

namespace CelesteEditor.Parameters.Numeric
{
    [CustomEditor(typeof(UIntValue))]
    public class UIntValueEditor : ParameterValueEditor<UIntValue>
    {
        #region GUI

        protected override void DrawRuntimeInspector()
        {
            Parameter.Value = CelesteEditorGUILayout.UIntField("Value", Parameter.Value);
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
