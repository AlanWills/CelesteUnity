using Celeste;
using CelesteEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem
{
    [CustomEditor(typeof(PlatformSettings), true)]
    [CanEditMultipleObjects]
    public class PlatformSettingsEditor : AdvancedEditor
    {
        #region GUI

        protected override void OnPrePropertiesGUI()
        {
            serializedObject.Update();

            PlatformSettings platformSettings = target as PlatformSettings;

            EditorGUILayout.LabelField("Init", CelesteGUIStyles.BoldLabel);

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Set Default Debug Values"))
                {
                    platformSettings.SetDefaultValues(true);
                }

                if (GUILayout.Button("Set Default Release Values"))
                {
                    platformSettings.SetDefaultValues(false);
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

#if USE_ADDRESSABLES
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Assets", CelesteGUIStyles.BoldLabel);

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Prepare Assets For Build"))
                {
                    platformSettings.PrepareAssetsForBuild();
                }

                if (GUILayout.Button("Build Assets"))
                {
                    platformSettings.BuildAssets();
                }
            }

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Prepare Assets For Update"))
                {
                    platformSettings.PrepareAssetsForUpdate();
                }

                if (GUILayout.Button("Update Assets"))
                {
                    platformSettings.UpdateAssets();
                }
            }
#endif

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Build", CelesteGUIStyles.BoldLabel);

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Prepare Build"))
                {
                    platformSettings.PrepareForBuild(platformSettings.GenerateBuildPlayerOptions());
                }

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

            serializedObject.ApplyModifiedProperties();
        }

#endregion
    }
}
