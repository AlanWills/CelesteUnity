using System.Linq;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public abstract class MultiPropertyAttribute : PropertyAttribute
    {
#if UNITY_EDITOR
        public List<MultiPropertyAttribute> stored = null;

        public virtual void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
        }

        public virtual void OnPreGUI(Rect position, SerializedProperty property) { }
        public virtual void OnPostGUI(Rect position, SerializedProperty property) { }

        public virtual bool IsVisible(SerializedProperty property) { return true; }
        public virtual float? GetPropertyHeight(SerializedProperty property, GUIContent label) { return null; }
#endif
    }
}