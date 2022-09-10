using Celeste.Parameters.Input;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Parameters.Input
{
    [CustomEditor(typeof(KeyCodeValue))]
    public class KeyCodeValueEditor : ParameterValueEditor<KeyCodeValue>
    {
        #region GUI

        protected override void DrawRuntimeInspector()
        {
            Parameter.Value = (KeyCode)EditorGUILayout.EnumPopup("Value", Parameter.Value);
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
