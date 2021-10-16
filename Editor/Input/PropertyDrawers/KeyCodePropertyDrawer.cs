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
        private string parseText = "A";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property) + 2 + GUI.skin.textField.CalcSize(new GUIContent(parseText)).y;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            float propertyHeight = EditorGUI.GetPropertyHeight(property);
            Rect propertyRect = position;
            propertyRect.height = propertyHeight;
            EditorGUI.PropertyField(propertyRect, property, label);

            propertyRect.y += propertyHeight + 2;

            GUIContent buttonContent = new GUIContent("Parse");
            float buttonWidth = GUI.skin.button.CalcSize(buttonContent).x;
            float totalWidth = propertyRect.width;
            propertyRect.width = totalWidth - buttonWidth - 5;
            parseText = EditorGUI.TextField(propertyRect, "KeyCode Text", parseText);

            propertyRect.x += totalWidth - buttonWidth;
            propertyRect.width = buttonWidth;
            
            if (GUI.Button(propertyRect, buttonContent))
            {
                if (Enum.TryParse(parseText, out KeyCode keyCode))
                {
                    property.enumValueIndex = Array.IndexOf(Enum.GetValues(typeof(KeyCode)), keyCode);
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
