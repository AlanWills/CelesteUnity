using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class LabelWidthAttribute : MultiPropertyAttribute
    {
        private float LabelWidth { get; set; }

        public LabelWidthAttribute(float labelWidth)
        {
            LabelWidth = labelWidth;
        }

#if UNITY_EDITOR
        public override void OnPreGUI(Rect position, SerializedProperty property)
        {
            EditorGUIUtility.labelWidth = LabelWidth;
        }

        public override void OnPostGUI(Rect position, SerializedProperty property)
        {
            EditorGUIUtility.labelWidth = 0;
        }
#endif
    }
}
