using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class LabelOverrideAttribute : MultiPropertyAttribute
    {
        #region Properties and Fields

        public GUIContent Label { get; }

        #endregion

        public LabelOverrideAttribute(string label)
        {
            Label = new GUIContent(label);
        }

#if UNITY_EDITOR
        public override float? GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetDefaultPropertyHeight(property, Label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, Label, true);
        }
#endif
    }
}