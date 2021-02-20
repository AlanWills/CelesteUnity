using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace CelesteEditor.Parameters.Numeric
{
    [CustomEditor(typeof(LongValue))]
    public class LongValueEditor : ParameterValueEditor<long>
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Parameter.Value = EditorGUILayout.LongField("Value", Parameter.Value);
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
