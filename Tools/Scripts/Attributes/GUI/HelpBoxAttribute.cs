using System;
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

    public class HelpBoxAttribute : MultiPropertyAttribute, IAdjustHeightAttribute, IPreGUIAttribute
    {
        public string HelpText { get; }
        public HelpBoxMessageType MessageType { get; }
        public string ValidationMethod { get; }

#if UNITY_EDITOR
        public float HelpBoxHeight
        {
            get
            {
                var helpBoxStyle = (UnityEngine.GUI.skin != null) ? UnityEngine.GUI.skin.GetStyle("helpbox") : null;
                if (helpBoxStyle != null)
                {
                    return 
                        HELP_BOX_PRE_PADDING + 
                        helpBoxStyle.CalcHeight(new GUIContent(HelpText), EditorGUIUtility.currentViewWidth) + 
                        HELP_BOX_POST_PADDING;
                }

                return 0;
            }
        }

        private bool validationMethodInfoObtained = false;
        private MethodInfo validationMethodInfo;

        private const float HELP_BOX_PRE_PADDING = 2;
        private const float HELP_BOX_POST_PADDING = 4;
#endif

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
        public float AdjustPropertyHeight(SerializedProperty property, GUIContent label, float height)
        {
            float delta = ShowHelpBox(property) ? HelpBoxHeight : 0;
            return height + delta;
        }

        public Rect OnPreGUI(Rect position, SerializedProperty property)
        {
            if (ShowHelpBox(property))
            {
                float helpBoxHeight = HelpBoxHeight;

                if (helpBoxHeight > 0)
                {
                    Rect helpBoxRect = new Rect(position);
                    helpBoxRect.y += HELP_BOX_PRE_PADDING;
                    helpBoxRect.height = helpBoxHeight - HELP_BOX_PRE_PADDING - HELP_BOX_POST_PADDING;

                    EditorGUI.HelpBox(helpBoxRect, HelpText, GetMessageType());

                    position.y += helpBoxHeight;
                    position.height -= helpBoxHeight;
                }
            }

            return position;
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

        private bool ShowHelpBox(SerializedProperty property)
        {
            MethodInfo validationMethod = TryObtainValidationMethodInfo(property);
            return validationMethod == null || !(bool)validationMethod.Invoke(property.serializedObject.targetObject, null);
        }

        private MethodInfo TryObtainValidationMethodInfo(SerializedProperty serializedProperty)
        {
            validationMethodInfoObtained |= string.IsNullOrEmpty(ValidationMethod);

            if (!validationMethodInfoObtained)
            {
                Type objectType = serializedProperty.serializedObject.targetObject.GetType();
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

            return validationMethodInfo;
        }
#endif
    }
}