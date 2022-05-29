using System.Collections;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class TimestampAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
        public override float? GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetDefaultPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            long timestamp = property.longValue;
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            string newDateTimeOffset = EditorGUI.DelayedTextField(position, label, dateTimeOffset.ToString());

            if (DateTimeOffset.TryParse(newDateTimeOffset, out DateTimeOffset offset))
            {
                property.longValue = offset.ToUnixTimeSeconds();
            }
        }
#endif
    }
}