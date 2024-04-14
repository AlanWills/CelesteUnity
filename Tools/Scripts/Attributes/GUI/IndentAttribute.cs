using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Celeste.Tools.Attributes.GUI
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class IndentAttribute : MultiPropertyAttribute, IPreGUIAttribute, IPostGUIAttribute
    {
#if UNITY_EDITOR
        public Rect OnPreGUI(Rect position, SerializedProperty property)
        {
            EditorGUI.indentLevel++;
            return position;
        }

        public Rect OnPostGUI(Rect position, SerializedProperty property)
        {
            EditorGUI.indentLevel--;
            return position;
        }
#endif
    }
}