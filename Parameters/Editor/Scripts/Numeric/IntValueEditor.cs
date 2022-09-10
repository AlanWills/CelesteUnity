using Celeste.Parameters;
using UnityEditor;

namespace CelesteEditor.Parameters.Numeric
{
    [CustomEditor(typeof(IntValue))]
    public class IntValueEditor : ParameterValueEditor<IntValue>
    {
        #region GUI

        protected override void DrawRuntimeInspector()
        {
            Parameter.Value = EditorGUILayout.IntField("Value", Parameter.Value);
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
