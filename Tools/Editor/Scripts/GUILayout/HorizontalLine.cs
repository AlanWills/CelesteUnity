using UnityEditor;
using UnityEngine;

namespace CelesteEditor
{
    public partial class CelesteEditorGUILayout
    {
        public static void HorizontalLine()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        public static void HorizontalLineWithSpace()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space();
        }
    }
}
