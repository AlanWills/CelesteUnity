using UnityEngine;

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
                object jsonObject = JsonUtility.FromJson<object>(property.stringValue);
                property.stringValue = JsonUtility.ToJson(jsonObject, true);
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