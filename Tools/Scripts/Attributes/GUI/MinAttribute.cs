using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MinAttribute : MultiPropertyAttribute, IGUIAttribute
    {
        public float Min { get; }

        public MinAttribute(int min)
        {
            Min = min;
        }

        public MinAttribute(float min)
        {
            Min = min;
        }

#if UNITY_EDITOR
        public Rect OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                int newValue = EditorGUI.DelayedIntField(position, label, property.intValue);
                property.intValue = Math.Max(newValue, (int)Min);
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                float newValue = EditorGUI.DelayedFloatField(position, label, property.floatValue);
                property.floatValue = Mathf.Max(newValue, Min);
            }

            return position;
        }
#endif
    }
}