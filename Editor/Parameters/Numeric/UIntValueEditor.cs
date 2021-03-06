﻿using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace CelesteEditor.Parameters.Numeric
{
    [CustomEditor(typeof(UIntValue))]
    public class UIntValueEditor : ParameterValueEditor<uint>
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Parameter.Value = CelesteEditorGUILayout.UIntField("Value", Parameter.Value);
            EditorUtility.SetDirty(Parameter);
        }

        #endregion
    }
}
