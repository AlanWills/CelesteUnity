using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class HideIfEnumAttribute : MultiPropertyAttribute, IVisibilityAttribute, IGetHeightAttribute
    {
        public string PropertyName { get; }
        public int Value { get; }

        public HideIfEnumAttribute(string propertyName, int value)
        {
            PropertyName = propertyName;
            Value = value;
        }

#if UNITY_EDITOR
        public bool IsVisible(SerializedProperty property)
        {
            return IsConditionallyEnabled(property);
        }

        public float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetDefaultPropertyHeight(property, label);
        }

        private bool IsConditionallyEnabled(SerializedProperty property)
        {
            string propertyPath = property.propertyPath;
            string conditionPath = propertyPath.Replace(property.name, PropertyName);
            SerializedProperty dependentProperty = property.serializedObject.FindProperty(conditionPath);

            return dependentProperty != null && dependentProperty.propertyType == SerializedPropertyType.Enum ? dependentProperty.intValue != Value : false;
        }
#endif
    }
}
