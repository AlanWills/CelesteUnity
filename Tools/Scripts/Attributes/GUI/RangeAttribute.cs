using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RangeAttribute : MultiPropertyAttribute, IGUIAttribute
    {
        public float Min { get; }
        public float Max { get; }

        public RangeAttribute(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public RangeAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }

#if UNITY_EDITOR
        public Rect OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                int newValue = EditorGUI.IntSlider(position, label, property.intValue, (int)Min, (int)Max);
                property.intValue = newValue;
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                float newValue = EditorGUI.Slider(position, label, property.floatValue, Min, Max);
                property.floatValue = newValue;
            }

            return position;
        }
#endif
    }
}