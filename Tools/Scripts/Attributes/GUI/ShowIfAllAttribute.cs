using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class ShowIfAllAttribute : MultiPropertyAttribute
    {
        private List<string> propertyNames = new List<string>();

        public ShowIfAllAttribute(params string[] propertyNames)
        {
            this.propertyNames.AddRange(propertyNames);
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
            bool areAllTrue = true;
            string propertyPath = property.propertyPath;

            foreach (string propertyName in propertyNames)
            {
                string conditionPath = propertyPath.Replace(property.name, propertyName);
                SerializedProperty dependentProperty = property.serializedObject.FindProperty(conditionPath);
                areAllTrue &= dependentProperty != null && dependentProperty.propertyType == SerializedPropertyType.Boolean ? dependentProperty.boolValue : false;
            }

            return areAllTrue;
        }
#endif
    }
}
