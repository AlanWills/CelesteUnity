using Celeste.Parameters;
using Celeste.Parameters.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Parameters.Input
{
    [CustomEditor(typeof(KeyCodeValue))]
    public class KeyCodeValueEditor : ParameterValueEditor<KeyCode>
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
