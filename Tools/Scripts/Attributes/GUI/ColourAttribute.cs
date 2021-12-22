using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class ColourAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
        public override void OnPreGUI(Rect position, SerializedProperty property)
        {
            UnityEngine.GUI.color = Color.blue;
        }
        public override void OnPostGUI(Rect position, SerializedProperty property)
        {
            UnityEngine.GUI.color = Color.black;
        }
#endif
    }
}