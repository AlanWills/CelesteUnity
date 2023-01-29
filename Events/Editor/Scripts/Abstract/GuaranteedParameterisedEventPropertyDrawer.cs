using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Events
{
    public class GuaranteedParameterisedEventPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GuaranteedEventPropertyDrawer.DrawBakedEvent(position, property, label);
        }
    }
}
