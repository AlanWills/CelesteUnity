﻿using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class InlineDataInInspectorAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
        #region Properties and Fields

        private const float TOP_SPACING = 3;
        private const float TITLE_SPACING = 5;
        private const float PROPERTY_SPACING = 2;
        private const float BOTTOM_SPACING = 5;

        #endregion

        public override float? GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float propertyHeight = TOP_SPACING + CelesteGUIStyles.BoldLabel.CalcSize(label).y + TITLE_SPACING;

            foreach (SerializedProperty visibleChild in VisibleChildren(property))
            {
                propertyHeight += EditorGUI.GetPropertyHeight(visibleChild, true) + PROPERTY_SPACING;
            }

            propertyHeight += BOTTOM_SPACING;

            return propertyHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect drawRect = new Rect(position);
            drawRect.y += TOP_SPACING;

            GUIStyle style = CelesteGUIStyles.BoldLabel;
            float labelHeight = style.CalcSize(label).y;
            drawRect.height = labelHeight;
            EditorGUI.LabelField(drawRect, label, style);
            drawRect.y += labelHeight + TITLE_SPACING;

            ++EditorGUI.indentLevel;

            foreach (SerializedProperty visibleChild in VisibleChildren(property))
            {
                float childHeight = EditorGUI.GetPropertyHeight(visibleChild, true);
                drawRect.height = childHeight;
                EditorGUI.PropertyField(drawRect, visibleChild, true);
                drawRect.y += childHeight + PROPERTY_SPACING;
            }

            --EditorGUI.indentLevel;
        }

        private IEnumerable<SerializedProperty> VisibleChildren(SerializedProperty property)
        {
            SerializedProperty nextSiblingProperty = property.Copy();
            nextSiblingProperty.NextVisible(false);

            if (property.NextVisible(true))
            {
                do
                {
                    if (SerializedProperty.EqualContents(property, nextSiblingProperty))
                        yield break;

                    yield return property;
                }
                while (property.NextVisible(false));
            }
        }
#endif
    }
}