using UnityEngine;
using Newtonsoft.Json.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class JsonAttribute : MultiPropertyAttribute, IGetHeightAttribute, IGUIAttribute
    {
#if UNITY_EDITOR
        public float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return UnityEngine.GUI.skin.label.CalcSize(new GUIContent(property.stringValue)).y;
        }

        public Rect OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUIContent buttonContent = new GUIContent("Prettify");
            Rect buttonPosition = new Rect(position);
            buttonPosition.width = 80;
            buttonPosition.height = UnityEngine.GUI.skin.label.CalcHeight(buttonContent, 80);

            if (UnityEngine.GUI.Button(buttonPosition, buttonContent))
            {
                JToken jt = JToken.Parse(property.stringValue);
                property.stringValue = jt.ToString(Newtonsoft.Json.Formatting.Indented);
            }

            Rect textAreaPosition = new Rect(position);
            textAreaPosition.width -= 80;
            textAreaPosition.x = position.x + buttonPosition.width;

            property.stringValue = EditorGUI.TextArea(textAreaPosition, property.stringValue);

            return position;
        }
#endif
    }
}