using Celeste;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem
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

            EditorGUILayout.LabelField("Init", CelesteGUIStyles.BoldLabel);

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Set Default Values"))
                {
                    platformSettings.SetDefaultValues();
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Platform", CelesteGUIStyles.BoldLabel);

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Apply"))
                {
                    platformSettings.Apply();
                }

                if (GUILayout.Button("Switch"))
                {
                    platformSettings.Switch();
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Assets", CelesteGUIStyles.BoldLabel);

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Prepare Assets"))
                {
                    platformSettings.PrepareAssets();
                }

                if (GUILayout.Button("Build Assets"))
                {
                    platformSettings.BuildAssets();
                }

                if (GUILayout.Button("Update Assets"))
                {
                    platformSettings.UpdateAssets();
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Build", CelesteGUIStyles.BoldLabel);

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Build"))
                {
                    platformSettings.BuildPlayer();
                }

                if (GUILayout.Button("Build And Run"))
                {
                    var buildPlayerOptions = platformSettings.GenerateBuildPlayerOptions();
                    buildPlayerOptions.options |= BuildOptions.AutoRunPlayer;
                    platformSettings.BuildPlayer(buildPlayerOptions);
                }

                if (GUILayout.Button("Increment Build Version"))
                {
                    platformSettings.IncrementBuild();
                }
            }

            EditorGUILayout.Space();

            base.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
