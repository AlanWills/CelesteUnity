using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class LayerAttribute : MultiPropertyAttribute, IGUIAttribute
    {
#if UNITY_EDITOR
        public Rect OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
            return position;
        }
#endif
    }
}
