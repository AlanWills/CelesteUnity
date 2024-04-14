using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public abstract class MultiPropertyAttribute : PropertyAttribute, IOrderableAttribute
    {
        public static bool s_disableRecursion;

        public bool attributesCached = false;
        public List<IAdjustHeightAttribute> adjustHeightAttributes = null;
        public List<IGetHeightAttribute> getHeightAttributes = null;
        public List<IGUIAttribute> guiAttributes = null;
        public List<IPostGUIAttribute> postGUIAttributes = null;
        public List<IPreGUIAttribute> preGUIAttributes = null;
        public List<IVisibilityAttribute> visibilityAttributes = null;

#if UNITY_EDITOR
        public static float GetDefaultPropertyHeight(SerializedProperty property, GUIContent label)
        {
            s_disableRecursion = true;
            float propertyHeight = EditorGUI.GetPropertyHeight(property, label, true);
            s_disableRecursion = false;

            return propertyHeight;
        }

        public static void DrawDefaultProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            s_disableRecursion = true;
            EditorGUI.PropertyField(position, property, label, true);
            s_disableRecursion = false;
        }
#endif
    }
}