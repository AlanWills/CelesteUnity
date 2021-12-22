using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class IndentAttribute : MultiPropertyAttribute
    {
#if UNITY_EDITOR
        public override void OnPreGUI(Rect position, SerializedProperty property)
        {
            EditorGUI.indentLevel++;
        }
        public override void OnPostGUI(Rect position, SerializedProperty property)
        {
            EditorGUI.indentLevel--;
        }
#endif
    }
}