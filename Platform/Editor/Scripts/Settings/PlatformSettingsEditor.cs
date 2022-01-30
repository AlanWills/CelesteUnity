using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Platform
{
    [CustomEditor(typeof(PlatformSettings), true)]
    [CanEditMultipleObjects]
    public class PlatformSettingsEditor : Editor
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            PlatformSettings platformSettings = target as PlatformSettings;

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Set Default Values", GUILayout.ExpandWidth(false)))
                {
                    platformSettings.SetDefaultValues();
                }
            }

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Apply", GUILayout.ExpandWidth(false)))
                {
                    platformSettings.Apply();
                }

                if (GUILayout.Button("Switch", GUILayout.ExpandWidth(false)))
                {
                    platformSettings.Switch();
                }
            }

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Build Player", GUILayout.ExpandWidth(false)))
                {
                    platformSettings.BuildPlayer();
                }

                if (GUILayout.Button("Increment Build", GUILayout.ExpandWidth(false)))
                {
                    platformSettings.IncrementBuild();
                }
            }

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Prepare Assets", GUILayout.ExpandWidth(false)))
                {
                    platformSettings.PrepareAssets();
                }

                if (GUILayout.Button("Build Assets", GUILayout.ExpandWidth(false)))
                {
                    platformSettings.BuildAssets();
                }

                if (GUILayout.Button("Update Assets", GUILayout.ExpandWidth(false)))
                {
                    platformSettings.UpdateAssets();
                }
            }

            base.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
