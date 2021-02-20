using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace CelesteEditor.Parameters.Numeric
{
    [CustomEditor(typeof(FloatValue))]
    public class FloatValueEditor : ParameterValueEditor<float>
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Parameter.Value = EditorGUILayout.FloatField("Value", Parameter.Value);
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
