using Celeste.Tilemaps.WaveFunctionCollapse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Tilemaps.WaveFunctionCollapse
{
    [CustomPropertyDrawer(typeof(Rule))]
    public class RulePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUILayout.BeginHorizontal();

            SerializedObject serializedRule = new SerializedObject(property.objectReferenceValue);
            serializedRule.Update();

            SerializedProperty directionProperty = serializedRule.FindProperty("direction");
            SerializedProperty otherTileProperty = serializedRule.FindProperty("otherTile");

            EditorGUILayout.PropertyField(directionProperty);
            EditorGUILayout.PropertyField(otherTileProperty, GUIContent.none);

            serializedRule.ApplyModifiedProperties();

            EditorGUILayout.EndHorizontal();
            EditorGUI.EndProperty();
        }
    }
}
