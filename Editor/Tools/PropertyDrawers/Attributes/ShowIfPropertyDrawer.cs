using Celeste.Attributes.GUI;
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
            ShowIfAttribute hideIfAttribute = attribute as ShowIfAttribute;
            SerializedProperty dependentProperty = property.serializedObject.FindProperty(hideIfAttribute.DependentName);

            return dependentProperty.boolValue ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute hideIfAttribute = attribute as ShowIfAttribute;
            SerializedProperty dependentProperty = property.serializedObject.FindProperty(hideIfAttribute.DependentName);

            EditorGUI.BeginProperty(position, label, property);

            if (dependentProperty.boolValue)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            EditorGUI.EndProperty();
        }
    }
}