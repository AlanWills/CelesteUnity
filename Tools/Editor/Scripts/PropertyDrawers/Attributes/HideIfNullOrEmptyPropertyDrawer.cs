using Celeste.Tools.Attributes.GUI;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(HideIfNullOrEmptyAttribute))]
    public class HideIfNullOrEmptyPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return IsConditionallyEnabled(property) ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (IsConditionallyEnabled(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            EditorGUI.EndProperty();
        }

        private bool IsConditionallyEnabled(SerializedProperty property)
        {
            HideIfNullOrEmptyAttribute hideIfNullOrEmptyAttribute = attribute as HideIfNullOrEmptyAttribute;
            string propertyPath = property.propertyPath;
            string conditionPath = propertyPath.Replace(property.name, hideIfNullOrEmptyAttribute.PropertyName);
            SerializedProperty dependentProperty = property.serializedObject.FindProperty(conditionPath);

            return dependentProperty != null && 
                (
                    (dependentProperty.isArray && dependentProperty.arraySize > 0 ) ||
                    (dependentProperty.propertyType == SerializedPropertyType.String && !string.IsNullOrEmpty(dependentProperty.stringValue))
                );
        }
    }
}