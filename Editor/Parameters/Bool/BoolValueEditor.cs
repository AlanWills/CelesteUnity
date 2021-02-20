using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace CelesteEditor.Parameters.Bool
{
    [CustomEditor(typeof(BoolValue))]
    public class BoolValueEditor : ParameterValueEditor<bool>
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Parameter.Value = EditorGUILayout.Toggle("Value", Parameter.Value);
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
