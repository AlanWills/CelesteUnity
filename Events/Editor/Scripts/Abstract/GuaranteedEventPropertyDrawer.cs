using Celeste.Events;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Events
{
    [CustomPropertyDrawer(typeof(GuaranteedEvent))]
    public class GuaranteedEventPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawBakedEvent(position, property, label);
        }

        public static void DrawBakedEvent(Rect position, SerializedProperty property, GUIContent label)
        {
            using (var propertyScope = new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUILayout.PropertyField(property.FindPropertyRelative("bakedEvent"));
            }
        }
    }
}
