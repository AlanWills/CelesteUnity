using Celeste.UI.Skin;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UI.Skin
{
    [CustomEditor(typeof(UISkinText))]
    public class UISkinTextEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                base.OnInspectorGUI();

                UISkinText uiSkinText = target as UISkinText;

                if (changeScope.changed | GUILayout.Button("Apply"))
                {
                    uiSkinText.Apply();
                }
            }
        }
    }
}