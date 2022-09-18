using Celeste.Parameters;
using UnityEditor;

namespace CelesteEditor.Parameters.Numeric
{
    [CustomEditor(typeof(FloatValue))]
    public class FloatValueEditor : ParameterValueEditor<FloatValue>
    {
        #region GUI

        protected override void DrawRuntimeInspector()
        {
            Parameter.Value = EditorGUILayout.FloatField("Value", Parameter.Value);
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
