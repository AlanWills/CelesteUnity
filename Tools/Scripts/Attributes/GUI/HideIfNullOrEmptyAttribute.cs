using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class HideIfNullOrEmptyAttribute : MultiPropertyAttribute, IVisibilityAttribute, IGetHeightAttribute
    {
        public string PropertyName { get; private set; }

        public HideIfNullOrEmptyAttribute(string propertyName)
        {
            PropertyName = propertyName;
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

            return dependentProperty != null &&
                (
                    (dependentProperty.isArray && dependentProperty.arraySize > 0) ||
                    (dependentProperty.propertyType == SerializedPropertyType.String && !string.IsNullOrEmpty(dependentProperty.stringValue))
                );
        }
#endif
    }
}
