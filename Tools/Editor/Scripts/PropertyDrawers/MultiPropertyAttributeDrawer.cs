using Celeste.Tools.Attributes.GUI;
using System.Collections.Generic;
using System.Linq;
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

            // Get the attribute list, sorted by "order".
            if (multiPropertyAttribute.stored == null)
            {
                List<MultiPropertyAttribute> newStored = new List<MultiPropertyAttribute>();
                foreach (var attr in fieldInfo.GetCustomAttributes(false))
                {
                    if (attr is MultiPropertyAttribute)
                    {
                        newStored.Add(attr as MultiPropertyAttribute);
                    }
                }

                newStored.Sort((a, b) => b.order - a.order);
                multiPropertyAttribute.stored = newStored;
            }

            return multiPropertyAttribute;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            MultiPropertyAttribute multiPropertyAttribute = RetrieveAttributes();
            var storedAttributes = multiPropertyAttribute.stored;

            // If the attribute is invisible, regain the standard vertical spacing.
            for (int i = 0, n = storedAttributes.Count; i < n; ++i)
            {
                if (!storedAttributes[i].IsVisible(property))
                    return -EditorGUIUtility.standardVerticalSpacing;
            }

            // In case no attribute returns a modified height, return the property's default one:
            float height = base.GetPropertyHeight(property, label);

            // Check if any of the attributes wants to modify height:
            for (int i = 0, n = storedAttributes.Count; i < n; ++i)
            {
                var tempheight = storedAttributes[i].GetPropertyHeight(property, label);
                if (tempheight.HasValue)
                {
                    height = Mathf.Max(height, tempheight.Value);
                }
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                MultiPropertyAttribute multiPropertyAttribute = RetrieveAttributes();
                var storedAttributes = multiPropertyAttribute.stored;

                // Calls to IsVisible. If it returns false for any attribute, the property will not be rendered.
                for (int i = 0, n = storedAttributes.Count; i < n; ++i)
                {
                    if (!storedAttributes[i].IsVisible(property)) return;
                }

                // Calls to OnPreRender before the last attribute draws the UI.
                for (int i = 0, n = storedAttributes.Count; i < n; ++i)
                {
                    storedAttributes[i].OnPreGUI(position, property);
                }

                // The last attribute is in charge of actually drawing something:
                multiPropertyAttribute.stored[multiPropertyAttribute.stored.Count - 1].OnGUI(position, property, label);

                // Calls to OnPostRender after the last attribute draws the UI. These are called in reverse order.
                for (int i = storedAttributes.Count - 1; i >= 0; --i)
                {
                    storedAttributes[i].OnPostGUI(position, property);
                }
            }
        }
    }
}