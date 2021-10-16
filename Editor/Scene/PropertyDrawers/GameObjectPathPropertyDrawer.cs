using Celeste.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.PropertyDrawers.Hierarchy
{
    [CustomPropertyDrawer(typeof(GameObjectPath))]
    public class GameObjectPathPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty pathProperty = property.FindPropertyRelative("path");
            EditorGUI.PropertyField(position, pathProperty, label, true);

            EditorGUI.EndProperty();
        }
    }
}
