using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor
{
    public partial class CelesteEditorGUILayout
    {
        public static uint UIntField(string label, uint value)
        {
            return UIntField(new GUIContent(label), value);
        }

        public static uint UIntField(GUIContent content, uint value)
        {
            int intValue = (int)value;
            intValue = EditorGUILayout.IntField(content, intValue);
            return intValue > 0 ? (uint)intValue : 0U;
        }
    }
}
