using UnityEngine;
using Newtonsoft.Json.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class JsonAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
        public override bool IsVisible(SerializedProperty property)
        {
            return true;
        }

        public override float? GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return UnityEngine.GUI.skin.label.CalcSize(new GUIContent(property.stringValue)).y;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect textAreaPosition = new Rect(position);
            textAreaPosition.width -= 80;

            property.stringValue = EditorGUI.TextArea(textAreaPosition, property.stringValue);

            GUIContent buttonContent = new GUIContent("Prettify");
            Rect buttonPosition = new Rect(position);
            buttonPosition.width = 80;
            buttonPosition.height = UnityEngine.GUI.skin.label.CalcHeight(buttonContent, 80);
            buttonPosition.x = position.x + textAreaPosition.width;

            if (UnityEngine.GUI.Button(buttonPosition, buttonContent))
            {
                JToken jt = JToken.Parse(property.stringValue);
                property.stringValue = jt.ToString(Newtonsoft.Json.Formatting.Indented);
            }
        }
#endif
    }
}