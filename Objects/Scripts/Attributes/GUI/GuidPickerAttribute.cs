using System;
using System.Collections.Generic;
using UnityEngine;
using Celeste.Tools.Attributes.GUI;
using Celeste.Tools;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Objects.Attributes.GUI
{
    public class GuidPickerAttribute : MultiPropertyAttribute, IGUIAttribute
    {
        #region Properties and Fields
        
        private Type Type { get; }

        private const float CHOOSE_BUTTON_WIDTH = 80;
        private const float CHOOSE_BUTTON_SPACING = 4;

        private static Dictionary<string, bool> propertyIsPickingLookup = new();
        
        #endregion

        public GuidPickerAttribute(Type type)
        {
            Type = type;
        }

#if UNITY_EDITOR
        public Rect OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect guidPosition = new Rect(position);
            guidPosition.width -= CHOOSE_BUTTON_WIDTH;
            guidPosition.width -= CHOOSE_BUTTON_SPACING;

            property.intValue = EditorGUI.IntField(guidPosition, label, property.intValue);

            GUIContent chooseContent = new GUIContent("Choose");
            Rect choosePosition = new Rect(position);
            choosePosition.width = CHOOSE_BUTTON_WIDTH;
            choosePosition.height = UnityEngine.GUI.skin.label.CalcHeight(chooseContent, 80);
            choosePosition.x = position.x + guidPosition.width + CHOOSE_BUTTON_SPACING;

            if (UnityEngine.GUI.Button(choosePosition, chooseContent))
            {
                propertyIsPickingLookup[property.propertyPath] = true;
                int controlIdWhichOpenedPicker = GUIUtility.GetControlID(property.propertyPath.GetHashCode(), FocusType.Passive, position);
                EditorGUIUtility.ShowObjectPicker<UnityEngine.Object>(null, false, $"t:{Type.Name}", controlIdWhichOpenedPicker);
            }

            string commandName = Event.current.commandName;

            if (propertyIsPickingLookup.TryGetValue(property.propertyPath, out bool isThisPropertyPicking) &&
                isThisPropertyPicking &&
                (string.CompareOrdinal(commandName, "ObjectSelectorUpdated") == 0 ||
                 string.CompareOrdinal(commandName, "ObjectSelectorClosed") == 0))
            {
                if (EditorGUIUtility.GetObjectPickerObject() is IIntGuid guid)
                {
                    property.intValue = guid.Guid;
                    property.serializedObject.ApplyModifiedProperties();
                }

                propertyIsPickingLookup[property.propertyPath] = false;
            }

            return position;
        }
#endif
    }
}