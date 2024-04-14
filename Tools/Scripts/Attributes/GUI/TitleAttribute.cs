using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TitleAttribute : MultiPropertyAttribute, IAdjustHeightAttribute, IPreGUIAttribute
    {
        public string Title { get; }
        public int Spacing { get; }

        public TitleAttribute(string title, int spacing = 10)
        {
            Title = title;
            Spacing = spacing;
        }

#if UNITY_EDITOR
        public float AdjustPropertyHeight(SerializedProperty property, GUIContent label, float height)
        {
            return height += CelesteGUIStyles.BoldLabel.CalcSize(new GUIContent(Title)).y + Spacing;
        }

        public Rect OnPreGUI(Rect position, SerializedProperty property)
        {
            GUIContent titleContent = new GUIContent(Title);
            Rect titlePosition = new Rect(position);
            titlePosition.y += Spacing;
            titlePosition.height = CelesteGUIStyles.BoldLabel.CalcSize(titleContent).y;

            EditorGUI.LabelField(titlePosition, titleContent, CelesteGUIStyles.BoldLabel);

            float heightDiff = titlePosition.height + Spacing;
            position.height -= heightDiff;
            position.y += heightDiff;

            return position;
        }
#endif
    }
}