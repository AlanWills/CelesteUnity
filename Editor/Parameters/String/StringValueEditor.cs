using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace CelesteEditor.Parameters.String
{
    [CustomEditor(typeof(StringValue))]
    public class StringValueEditor : ParameterValueEditor<string>
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PrefixLabel("Default Value");
            Parameter.DefaultValue = EditorGUILayout.TextArea(Parameter.DefaultValue);

            EditorGUILayout.Space();

            EditorGUILayout.PrefixLabel("Value");
            Parameter.Value = EditorGUILayout.TextArea(Parameter.Value);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(Parameter);
            }
        }

        #endregion
    }
}
