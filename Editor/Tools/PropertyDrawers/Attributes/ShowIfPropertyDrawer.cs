using Celeste.Tools.Attributes.GUI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return IsVisible(property) ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (IsVisible(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            EditorGUI.EndProperty();
        }

        private bool IsVisible(SerializedProperty property)
        {
            ShowIfAttribute showIfAttribute = attribute as ShowIfAttribute;
            string propertyPath = property.propertyPath;
            string conditionPath = propertyPath.Replace(property.name, showIfAttribute.DependentName);
            SerializedProperty dependentProperty = property.serializedObject.FindProperty(conditionPath);

            return dependentProperty != null && dependentProperty.propertyType == SerializedPropertyType.Boolean ? dependentProperty.boolValue : false;
        }
    }
}