using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class ShowIfAttribute : MultiPropertyAttribute
    {
        public string PropertyName { get; private set; }

        public ShowIfAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

#if UNITY_EDITOR
        public override bool IsVisible(SerializedProperty property)
        {
            return IsConditionallyEnabled(property);
        }

        public override float? GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        private bool IsConditionallyEnabled(SerializedProperty property)
        {
            string propertyPath = property.propertyPath;
            string conditionPath = propertyPath.Replace(property.name, PropertyName);
            SerializedProperty dependentProperty = property.serializedObject.FindProperty(conditionPath);

            return dependentProperty != null && dependentProperty.propertyType == SerializedPropertyType.Boolean ? dependentProperty.boolValue : false;
        }
#endif
    }
}
