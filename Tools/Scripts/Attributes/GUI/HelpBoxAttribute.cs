using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public enum HelpBoxMessageType
    {
        None,
        Info,
        Warning,
        Error
    }

    public class HelpBoxAttribute : MultiPropertyAttribute
    {
        public string HelpText { get; }
        public HelpBoxMessageType MessageType { get; }
        public string ValidationMethod { get; }

        private bool validationMethodInfoObtained = false;
        private MethodInfo validationMethodInfo;

        public HelpBoxAttribute(string helpText, HelpBoxMessageType messageType) :
            this(helpText, messageType, string.Empty)
        {
        }

        public HelpBoxAttribute(string helpText, HelpBoxMessageType messageType, string validationMethod)
        {
            HelpText = helpText;
            MessageType = messageType;
            ValidationMethod = validationMethod;
        }

#if UNITY_EDITOR
        public override bool IsVisible(SerializedProperty property)
        {
            if (string.IsNullOrEmpty(ValidationMethod))
            {
                return true;
            }

            if (!validationMethodInfoObtained)
            {
                Type objectType = property.serializedObject.targetObject.GetType();
                validationMethodInfo = objectType.GetMethod(ValidationMethod);
                validationMethodInfoObtained = true;

                if (validationMethodInfo == null)
                {
                    Debug.LogAssertion($"Could not find Validation Method '{ValidationMethod}' on object {objectType.Name}.");
                }
                else
                {
                    var parameters = validationMethodInfo.GetParameters();
                    Debug.Assert(parameters == null || parameters.Length == 0, $"Validation Method does not support parameters.");
                    Debug.Assert(validationMethodInfo.ReturnType == typeof(bool), $"Validation Method does not return '{typeof(bool)}'.");
                }
            }

            return validationMethodInfo != null && (bool)validationMethodInfo.Invoke(property.serializedObject.targetObject, null);
        }

        public override float? GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var helpBoxStyle = (UnityEngine.GUI.skin != null) ? UnityEngine.GUI.skin.GetStyle("helpbox") : null;
            if (helpBoxStyle != null)
            {
                return GetDefaultPropertyHeight(property, label) + Mathf.Max(40f, helpBoxStyle.CalcHeight(new GUIContent(HelpText), EditorGUIUtility.currentViewWidth) + 4);
            }

            return null;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);

            EditorGUI.HelpBox(position, HelpText, GetMessageType());
        }

        private MessageType GetMessageType()
        {
            switch (MessageType)
            {
                case HelpBoxMessageType.None: return UnityEditor.MessageType.None;
                case HelpBoxMessageType.Info: return UnityEditor.MessageType.Info;
                case HelpBoxMessageType.Warning: return UnityEditor.MessageType.Warning;
                case HelpBoxMessageType.Error: return UnityEditor.MessageType.Error;
                default:
                    Debug.LogAssertion($"Unhandled message type '{MessageType}'.");
                    return UnityEditor.MessageType.None;
            }
        }
#endif
    }
}