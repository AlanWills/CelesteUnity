using Celeste.Events;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Events
{
    [CustomPropertyDrawer(typeof(GuaranteedEvent))]
    public class GuaranteedEventPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty bakedEventProperty = property.FindPropertyRelative("bakedEvent");
            return EditorGUI.GetPropertyHeight(bakedEventProperty, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawBakedEvent(position, property, label);
        }

        public static void DrawBakedEvent(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                SerializedProperty bakedEventProperty = property.FindPropertyRelative("bakedEvent");
                EditorGUI.PropertyField(position, bakedEventProperty, label, true);
            }
        }
    }
}
