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
    [CustomPropertyDrawer(typeof(Direction))]
    public class DirectionPropertyDrawer : PropertyDrawer
    {
        public static readonly string[] DISPLAY_NAMES =
        {
            "Can Go To The Left Of",
            "Can Go Above",
            "Can Go To The Right Of",
            "Can Go Below",
            "Can Go Above Left Of",
            "Can Go Above Right Of",
            "Can Go Below Right Of",
            "Can Go Below Left Of"
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            property.enumValueIndex = EditorGUI.Popup(position, property.enumValueIndex, DISPLAY_NAMES);

            EditorGUI.EndProperty();
        }

        public static Direction GUI(Direction direction)
        {
            return (Direction)EditorGUILayout.Popup((int)direction, DISPLAY_NAMES);
        }
    }
}
