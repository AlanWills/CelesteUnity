using UnityEngine;

#if UNITY_EDITOR
using System;
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class StringAsSystemGuidAttribute : MultiPropertyAttribute, IGUIAttribute
    {
#if UNITY_EDITOR
        private const float GENERATE_BUTTON_SPACING = 5;
        
        public Rect OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUIContent generateButtonContent = new GUIContent("Generate");
            float generateButtonWidth = UnityEngine.GUI.skin.button.CalcSize(generateButtonContent).x;
            
            Rect textAreaPosition = new Rect(position);
            textAreaPosition.width -= generateButtonWidth;
            textAreaPosition.width -= GENERATE_BUTTON_SPACING;

            string newValue = EditorGUI.DelayedTextField(textAreaPosition, label, property.stringValue);
            if (Guid.TryParse(newValue, out _))
            {
                property.stringValue = newValue;
            }
            else
            {
                Debug.LogError($"{newValue} is not a valid GUID value and so will be ignored.");
            }
            
            Rect generateButtonPosition = new Rect(position);
            generateButtonPosition.width = generateButtonWidth;
            generateButtonPosition.x += textAreaPosition.width;
            generateButtonPosition.x += GENERATE_BUTTON_SPACING;

            if (string.IsNullOrEmpty(property.stringValue) | UnityEngine.GUI.Button(generateButtonPosition, generateButtonContent))
            {
                property.stringValue = Guid.NewGuid().ToString();
            }

            return position;
        }
#endif
    }
}