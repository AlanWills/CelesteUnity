using System;
using UnityEngine;
using Celeste.Tools.Attributes.GUI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Objects.Attributes.GUI
{
    public class GuidPickerAttribute : MultiPropertyAttribute
    {
        public Type Type { get; }

        public GuidPickerAttribute(Type type)
        {
            Type = type;
        }

#if UNITY_EDITOR
        public override bool IsVisible(SerializedProperty property)
        {
            return true;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect guidPosition = new Rect(position);
            guidPosition.width -= 80;

            property.intValue = EditorGUI.IntField(guidPosition, label, property.intValue);

            GUIContent chooseContent = new GUIContent("Choose");
            Rect choosePosition = new Rect(position);
            choosePosition.width = 80;
            choosePosition.height = UnityEngine.GUI.skin.label.CalcHeight(chooseContent, 80);
            choosePosition.x = position.x + guidPosition.width;

            if (UnityEngine.GUI.Button(choosePosition, chooseContent))
            {
                EditorGUIUtility.ShowObjectPicker<UnityEngine.Object>(null, false, $"t:{Type.Name}", EditorGUIUtility.GetControlID(FocusType.Passive));
            }

            string commandName = Event.current.commandName;

            if (string.CompareOrdinal(commandName, "ObjectSelectorUpdated") == 0 ||
                string.CompareOrdinal(commandName, "ObjectSelectorClosed") == 0)
            {
                IIntGuid guid = EditorGUIUtility.GetObjectPickerObject() as IIntGuid;
                
                if (guid != null)
                {
                    property.intValue = guid.Guid;
                }
            }
        }
#endif
    }
}