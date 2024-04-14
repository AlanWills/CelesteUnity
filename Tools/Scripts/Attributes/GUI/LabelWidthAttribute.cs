using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class LabelWidthAttribute : MultiPropertyAttribute, IPreGUIAttribute, IPostGUIAttribute
    {
        private float LabelWidth { get; set; }

        public LabelWidthAttribute(float labelWidth)
        {
            LabelWidth = labelWidth;
        }

#if UNITY_EDITOR
        public Rect OnPreGUI(Rect position, SerializedProperty property)
        {
            EditorGUIUtility.labelWidth = LabelWidth;
            return position;
        }

        public Rect OnPostGUI(Rect position, SerializedProperty property)
        {
            EditorGUIUtility.labelWidth = 0;
            return position;
        }
#endif
    }
}
