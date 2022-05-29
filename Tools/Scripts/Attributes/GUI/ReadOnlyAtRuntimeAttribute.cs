using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    public class ReadOnlyAtRuntimeAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
        public override float? GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetDefaultPropertyHeight(property, label);
        }

        public override void OnPreGUI(Rect position, SerializedProperty property)
        {
            base.OnPreGUI(position, property);

            UnityEngine.GUI.enabled = !Application.isPlaying;
        }

        public override void OnPostGUI(Rect position, SerializedProperty property)
        {
            base.OnPostGUI(position, property);

            UnityEngine.GUI.enabled = true;
        }
#endif
    }
}
