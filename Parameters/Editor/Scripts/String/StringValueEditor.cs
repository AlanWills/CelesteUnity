using Celeste.Parameters;
using UnityEditor;

namespace CelesteEditor.Parameters.String
{
    [CustomEditor(typeof(StringValue))]
    public class StringValueEditor : ParameterValueEditor<StringValue>
    {
        #region GUI

        protected override void DrawRuntimeInspector()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PrefixLabel("Value");
            Parameter.Value = EditorGUILayout.TextArea(Parameter.Value);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(Parameter);
            }

            EditorGUILayout.EndHorizontal();
        }

        #endregion
    }
}
