using Celeste.Tools.Attributes.GUI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(HideIfEnumAttribute))]
    public class HideIfEnumPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return !IsConditionallyEnabled(property) ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (!IsConditionallyEnabled(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            EditorGUI.EndProperty();
        }

        private bool IsConditionallyEnabled(SerializedProperty property)
        {
            HideIfEnumAttribute hideIfEnumAttribute = attribute as HideIfEnumAttribute;
            string propertyPath = property.propertyPath;
            string conditionPath = propertyPath.Replace(property.name, hideIfEnumAttribute.PropertyName);
            SerializedProperty dependentProperty = property.serializedObject.FindProperty(conditionPath);

            return dependentProperty != null && dependentProperty.propertyType == SerializedPropertyType.Enum ? dependentProperty.intValue == hideIfEnumAttribute.Value : false;
        }
    }
}