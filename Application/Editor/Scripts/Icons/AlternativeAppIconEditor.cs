using Celeste.Application;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Application
{
    [CustomEditor(typeof(AlternativeAppIcon))]
    public class AlternativeAppIconEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            AlternativeAppIcon alternativeAppIcon = target as AlternativeAppIcon;

            if (alternativeAppIcon.HasSource &&
                GUILayout.Button("Generate All Icons From Source"))
            {
                alternativeAppIcon.GenerateAndSaveIcons();
            }

            base.OnInspectorGUI();
        }
    }
}