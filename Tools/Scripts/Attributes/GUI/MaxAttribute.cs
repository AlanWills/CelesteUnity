using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MaxAttribute : MultiPropertyAttribute, IGUIAttribute
    {
        public float Max { get; }

        public MaxAttribute(int max)
        {
            Max = max;
        }

        public MaxAttribute(float max)
        {
            Max = max;
        }

#if UNITY_EDITOR
        public Rect OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                int newValue = EditorGUI.DelayedIntField(position, label, property.intValue);
                property.intValue = Math.Min(newValue, (int)Max);
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                float newValue = EditorGUI.DelayedFloatField(position, label, property.floatValue);
                property.floatValue = Mathf.Min(newValue, Max);
            }

            return position;
        }
#endif
    }
}