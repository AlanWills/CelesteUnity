using UnityEditor;
using UnityEngine;

namespace CelesteEditor
{
    public partial class CelesteEditorGUILayout
    {
        public static void TightLabel(string label)
        {
            TightLabel(label, 0);
        }

        public static void TightLabel(string label, float padding)
        {
            GUIContent labelContent = new GUIContent(label);
            EditorGUILayout.LabelField(label, GUILayout.Width(GUI.skin.label.CalcSize(labelContent).x + padding));
        }
    }
}
