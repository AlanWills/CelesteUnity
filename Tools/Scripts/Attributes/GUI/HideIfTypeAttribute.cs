using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class HideIfTypeAttribute : MultiPropertyAttribute, IVisibilityAttribute, IGetHeightAttribute
    {
        private string propertyName;
        private int targetType;

        public HideIfTypeAttribute(string propertyName, int targetType)
        {
            this.propertyName = propertyName;
            this.targetType = targetType;
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
            string conditionPath = propertyPath.Replace(property.name, propertyName);
            SerializedProperty dependentProperty = property.serializedObject.FindProperty(conditionPath);

            return dependentProperty != null && (int)dependentProperty.propertyType == targetType;
        }
#endif
    }
}
