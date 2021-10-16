using Celeste.Tools.Attributes.GUI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ReadOnlyAtRuntimeAttribute))]
    public class ReadOnlyAtRuntimePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            GUI.enabled = !Application.isPlaying;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;

            EditorGUI.EndProperty();
        }
    }
}