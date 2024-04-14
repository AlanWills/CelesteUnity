using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Tools.PropertyDrawers.Attributes
{
    [CustomPropertyDrawer(typeof(MultiPropertyAttribute), true)]
    public class MultiPropertyDrawer : PropertyDrawer
    {
        private MultiPropertyAttribute RetrieveAttributes()
        {
            MultiPropertyAttribute multiPropertyAttribute = attribute as MultiPropertyAttribute;

            if (!multiPropertyAttribute.attributesCached)
            {
                object[] customAttributes = fieldInfo.GetCustomAttributes(false);

                multiPropertyAttribute.adjustHeightAttributes = RetrieveAttributes<IAdjustHeightAttribute>(customAttributes);
                multiPropertyAttribute.getHeightAttributes = RetrieveAttributes<IGetHeightAttribute>(customAttributes);
                multiPropertyAttribute.guiAttributes = RetrieveAttributes<IGUIAttribute>(customAttributes);
                multiPropertyAttribute.preGUIAttributes = RetrieveAttributes<IPreGUIAttribute>(customAttributes);
                multiPropertyAttribute.postGUIAttributes = RetrieveAttributes<IPostGUIAttribute>(customAttributes);
                multiPropertyAttribute.visibilityAttributes = RetrieveAttributes<IVisibilityAttribute>(customAttributes);

                multiPropertyAttribute.attributesCached = true;
            }

            return multiPropertyAttribute;
        }

        private List<T> RetrieveAttributes<T>(object[] customAttributes) where T : IOrderableAttribute
        {
            List<T> attributes = new List<T>();

            foreach (var attr in customAttributes)
            {
                if (attr is T tAttr)
                {
                    attributes.Add(tAttr);
                }
            }

            attributes.Sort((a, b) => b.order - a.order);
            return attributes;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (MultiPropertyAttribute.s_disableRecursion)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }

            MultiPropertyAttribute multiPropertyAttribute = RetrieveAttributes();

            // If the attribute is invisible, regain the standard vertical spacing.
            for (int i = 0, n = multiPropertyAttribute.visibilityAttributes.Count; i < n; ++i)
            {
                if (!multiPropertyAttribute.visibilityAttributes[i].IsVisible(property))
                    return -EditorGUIUtility.standardVerticalSpacing;
            }

            // In case no attribute returns a modified height, return the property's default one:
            float height = 0;

            if (multiPropertyAttribute.getHeightAttributes.Count == 0)
            {
                height = MultiPropertyAttribute.GetDefaultPropertyHeight(property, label);
            }
            else
            {
                // Check if any of the attributes wants to set the height - largest wins:
                for (int i = 0, n = multiPropertyAttribute.getHeightAttributes.Count; i < n; ++i)
                {
                    var tempheight = multiPropertyAttribute.getHeightAttributes[i].GetPropertyHeight(property, label);
                    height = Mathf.Max(height, tempheight);
                }
            }

            for (int i = 0, n = multiPropertyAttribute.adjustHeightAttributes.Count; i < n; ++i)
            {
                height = multiPropertyAttribute.adjustHeightAttributes[i].AdjustPropertyHeight(property, label, height);
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (MultiPropertyAttribute.s_disableRecursion)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            MultiPropertyAttribute multiPropertyAttribute = RetrieveAttributes();

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                // Calls to IsVisible. If it returns false for any attribute, the property will not be rendered.
                for (int i = 0, n = multiPropertyAttribute.visibilityAttributes.Count; i < n; ++i)
                {
                    if (!multiPropertyAttribute.visibilityAttributes[i].IsVisible(property)) return;
                }

                // Calls to OnPreRender before the last attribute draws the UI.
                for (int i = 0, n = multiPropertyAttribute.preGUIAttributes.Count; i < n; ++i)
                {
                    position = multiPropertyAttribute.preGUIAttributes[i].OnPreGUI(position, property);
                }

                if (multiPropertyAttribute.guiAttributes.Count == 0)
                {
                    MultiPropertyAttribute.DrawDefaultProperty(position, property, label);
                }
                else
                {
                    for (int i = 0, n = multiPropertyAttribute.guiAttributes.Count; i < n; ++i)
                    {
                        position = multiPropertyAttribute.guiAttributes[i].OnGUI(position, property, label);
                    }
                }

                for (int i = multiPropertyAttribute.postGUIAttributes.Count - 1; i >= 0; --i)
                {
                    position = multiPropertyAttribute.postGUIAttributes[i].OnPostGUI(position, property);
                }
            }
        }
    }
}