using UnityEngine;
using Celeste.Tools.Attributes.GUI;
using Celeste.Tools;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Localisation
{
    public class LocalisationPreviewAttribute : MultiPropertyAttribute, IGetHeightAttribute, IGUIAttribute
    {
#if UNITY_EDITOR
        private const float spacing = 5;
        private const int textAreaLineHeights = 2;

        public float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float propertyHeight = EditorGUIUtility.singleLineHeight;
            LocalisationKey localisationKey = property.objectReferenceValue as LocalisationKey;

            if (localisationKey != null)
            {
                float heightForText = GetPreviewTextHeight(localisationKey.Fallback, out _);
                propertyHeight += (spacing + heightForText);
            }

            return propertyHeight;
        }

        public Rect OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LocalisationKey localisationKey = property.objectReferenceValue as LocalisationKey;
            Rect localisationKeyPosition = new Rect(position);

            if (localisationKey != null)
            {
                Rect localisationPreviewPosition = new Rect(position) { height = GetPreviewTextHeight(localisationKey.Fallback, out bool maxHeightReached) };

                if (maxHeightReached)
                {
                    GUI.tooltip = localisationKey.Fallback;
                }

                EditorGUI.HelpBox(localisationPreviewPosition, localisationKey.Fallback, MessageType.None);
                GUI.tooltip = string.Empty;

                localisationKeyPosition.height = EditorGUIUtility.singleLineHeight;
                localisationKeyPosition.y += localisationPreviewPosition.height + spacing;
            }

            GUIContent localisationKeyLabel = EditorGUIUtility.TrTempContent(property.displayName);
            localisationKeyLabel.tooltip = localisationKey?.Fallback ?? string.Empty;
            property.objectReferenceValue = EditorGUI.ObjectField(localisationKeyPosition, localisationKeyLabel, property.objectReferenceValue, typeof(LocalisationKey), false);

            return position;
        }

        private float GetPreviewTextHeight(string text, out bool maxSizeReached)
        {
            float heightForText = GUI.skin.box.CalcSize(EditorGUIUtility.TrTempContent(text)).y;
            float maxHeightForText = EditorGUIUtility.singleLineHeight * textAreaLineHeights;
            maxSizeReached = heightForText > maxHeightForText;

            return Mathf.Min(heightForText, maxHeightForText);
        }
#endif
    }
}
