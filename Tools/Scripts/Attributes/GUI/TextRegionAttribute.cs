using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

using System;



#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class TextRegionAttribute : MultiPropertyAttribute
    {
        public float MinHeight { get; }

        public TextRegionAttribute() : this(1)
        {
        }

        public TextRegionAttribute(float minHeight)
        {
            MinHeight = minHeight;
        }

        public TextRegionAttribute(int minHeightAsMultiplesOfLineHeight)
        {
#if UNITY_EDITOR
            MinHeight = EditorGUIUtility.singleLineHeight * minHeightAsMultiplesOfLineHeight;
#endif
        }

#if UNITY_EDITOR
        public override float? GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            GUIContent tempContent = EditorGUIUtility.TrTempContent(property.stringValue);
            float wrappedTextHeight = CelesteGUIStyles.WrappedTextArea.CalcSize(tempContent).y;
            return Mathf.Max(MinHeight, wrappedTextHeight);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.stringValue = EditorGUI.TextArea(position, property.stringValue, CelesteGUIStyles.WrappedTextArea);
        }
#endif
    }
}
