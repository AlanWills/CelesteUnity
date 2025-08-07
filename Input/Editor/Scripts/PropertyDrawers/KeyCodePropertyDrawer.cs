using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Input.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(KeyCode))]
    public class KeyCodePropertyDrawer : PropertyDrawer
    {
        private string parseText = string.Empty;
        private const float SPACING = 4;
        private const float TEXT_FIELD_WIDTH = 60;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            
            GUIContent buttonContent = new GUIContent("Parse");
            float buttonWidth = GUI.skin.button.CalcSize(buttonContent).x;

            Rect propertyRect = position;
            propertyRect.width -= (SPACING * 2 + TEXT_FIELD_WIDTH + buttonWidth);
            EditorGUI.PropertyField(propertyRect, property, label);

            Rect textFieldRect = propertyRect;
            textFieldRect.x += propertyRect.width + SPACING;
            textFieldRect.width = TEXT_FIELD_WIDTH;
            parseText = EditorGUI.TextField(textFieldRect, GUIContent.none, parseText);

            Rect parseButtonRect = textFieldRect;
            parseButtonRect.x += textFieldRect.width + SPACING;
            parseButtonRect.width = buttonWidth;
            
            if (GUI.Button(parseButtonRect, buttonContent))
            {
                if (Enum.TryParse(parseText, out KeyCode keyCode))
                {
                    property.enumValueIndex = Array.IndexOf(Enum.GetValues(typeof(KeyCode)), keyCode);
                    parseText = string.Empty;
                }
                else
                {
                    Debug.LogErrorFormat("Could not Parse: {0} as a KeyCode", parseText);
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
