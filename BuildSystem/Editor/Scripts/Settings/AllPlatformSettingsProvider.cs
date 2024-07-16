using Celeste;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CelesteEditor.BuildSystem
{
    public class AllPlatformSettingsProvider : SettingsProvider
    {
        #region Properties and Fields

        private SerializedObject allPlatformSettings;
        private SerializedProperty iOSDebugProperty;
        private SerializedProperty iOSReleaseProperty;
        private SerializedProperty androidDebugApkProperty;
        private SerializedProperty androidDebugBundleProperty;
        private SerializedProperty androidReleaseApkProperty;
        private SerializedProperty androidReleaseBundleProperty;
        private SerializedProperty windowsDebugProperty;
        private SerializedProperty windowsReleaseProperty;
        private SerializedProperty webGLDebugProperty;
        private SerializedProperty webGLReleaseProperty;

        #endregion

        public AllPlatformSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            // This function is called when the user clicks on the MyCustom element in the Settings window.
            allPlatformSettings = AllPlatformSettings.GetSerializedSettings();
            iOSDebugProperty = allPlatformSettings.FindProperty("m_iOSDebug");
            iOSReleaseProperty = allPlatformSettings.FindProperty("m_iOSRelease");
            androidDebugApkProperty = allPlatformSettings.FindProperty("m_androidDebugApk");
            androidDebugBundleProperty = allPlatformSettings.FindProperty("m_androidDebugBundle");
            androidReleaseApkProperty = allPlatformSettings.FindProperty("m_androidReleaseApk");
            androidReleaseBundleProperty = allPlatformSettings.FindProperty("m_androidReleaseBundle");
            windowsDebugProperty = allPlatformSettings.FindProperty("m_windowsDebug");
            windowsReleaseProperty = allPlatformSettings.FindProperty("m_windowsRelease");
            webGLDebugProperty = allPlatformSettings.FindProperty("m_webGLDebug");
            webGLReleaseProperty = allPlatformSettings.FindProperty("m_webGLRelease");
        }

        public override void OnGUI(string searchContext)
        {
            allPlatformSettings.Update();

            DrawAndroidSettings();
            DrawiOSSettings();
            DrawWindowsSettings();
            DrawWebGLSettings();

            allPlatformSettings.ApplyModifiedProperties();
        }

        private void DrawiOSSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("iOS", CelesteGUIStyles.BoldLabel);
            EditorGUILayout.Space();

            using (EditorGUI.IndentLevelScope indent = new EditorGUI.IndentLevelScope())
            {
                SettingsGUI(
                    iOSDebugProperty,
                    "iOS Debug",
                    () => AllPlatformSettings.FindOrCreateiOSSettingsAsset(AllPlatformSettings.iOSPlatformSettingsPath, "iOSDebug", true));

                SettingsGUI(
                    iOSReleaseProperty,
                    "iOS Release",
                    () => AllPlatformSettings.FindOrCreateiOSSettingsAsset(AllPlatformSettings.iOSPlatformSettingsPath, "iOSRelease", false));
            }
        }

        private void DrawAndroidSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Android", CelesteGUIStyles.BoldLabel);
            EditorGUILayout.Space();

            using (EditorGUI.IndentLevelScope indent = new EditorGUI.IndentLevelScope())
            {
                SettingsGUI(
                    androidDebugApkProperty,
                    "Android Debug Apk",
                    () => AllPlatformSettings.FindOrCreateAndroidSettingsAsset(
                        AllPlatformSettings.AndroidPlatformSettingsPath, 
                        "AndroidDebugApk", 
                        true, 
                        false));

                SettingsGUI(
                    androidDebugBundleProperty,
                    "Android Debug Bundle",
                    () => AllPlatformSettings.FindOrCreateAndroidSettingsAsset(
                        AllPlatformSettings.AndroidPlatformSettingsPath,
                        "AndroidDebugBundle",
                        true,
                        true));

                SettingsGUI(
                    androidReleaseApkProperty,
                    "Android Release Apk",
                    () => AllPlatformSettings.FindOrCreateAndroidSettingsAsset(
                        AllPlatformSettings.AndroidPlatformSettingsPath,
                        "AndroidReleaseApk",
                        false,
                        false));

                SettingsGUI(
                    androidReleaseBundleProperty,
                    "Android Release Bundle",
                    () => AllPlatformSettings.FindOrCreateAndroidSettingsAsset(
                        AllPlatformSettings.AndroidPlatformSettingsPath,
                        "AndroidReleaseBundle",
                        false,
                        true));
            }
        }

        private void DrawWindowsSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Windows", CelesteGUIStyles.BoldLabel);
            EditorGUILayout.Space();

            using (EditorGUI.IndentLevelScope indent = new EditorGUI.IndentLevelScope())
            {
                SettingsGUI(
                    windowsDebugProperty,
                    "Windows Debug",
                    () => AllPlatformSettings.FindOrCreateWindowsSettingsAsset(AllPlatformSettings.WindowsPlatformSettingsPath, "WindowsDebug", true));

                SettingsGUI(
                    windowsReleaseProperty,
                    "Windows Release",
                    () => AllPlatformSettings.FindOrCreateWindowsSettingsAsset(AllPlatformSettings.WindowsPlatformSettingsPath, "WindowsRelease", false));
            }
        }

        private void DrawWebGLSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("WebGL", CelesteGUIStyles.BoldLabel);
            EditorGUILayout.Space();

            using (EditorGUI.IndentLevelScope indent = new EditorGUI.IndentLevelScope())
            {
                SettingsGUI(
                    webGLDebugProperty,
                    "WebGL Debug",
                    () => AllPlatformSettings.FindOrCreateWebGLSettingsAsset(AllPlatformSettings.WebGLPlatformSettingsPath, "WebGLDebug", true));

                SettingsGUI(
                    webGLReleaseProperty,
                    "WebGL Release",
                    () => AllPlatformSettings.FindOrCreateWebGLSettingsAsset(AllPlatformSettings.WebGLPlatformSettingsPath, "WebGLRelease", false));
            }
        }

        private void SettingsGUI<T>(
            SerializedProperty settingsProperty,
            string settingsPropertyName,
            Func<T> findOrCreateFunctor) where T : PlatformSettings
        {
            if (settingsProperty.objectReferenceValue != null)
            {
                EditorGUILayout.PropertyField(settingsProperty, new GUIContent(settingsPropertyName));
            }
            else if (GUILayout.Button($"Find Or Create {settingsPropertyName} Settings", GUILayout.ExpandWidth(false)))
            {
                settingsProperty.objectReferenceValue = findOrCreateFunctor?.Invoke();
            }
        }

        #region Settings Provider

        public static bool IsSettingsAvailable()
        {
            return File.Exists(AllPlatformSettings.AllPlatformSettingsPath);
        }

        // Register the SettingsProvider
        [SettingsProvider]
        public static SettingsProvider CreateAllPlatformSettingsProvider()
        {
            if (IsSettingsAvailable())
            {
                var provider = new AllPlatformSettingsProvider("Project/Celeste/All Platform Settings", SettingsScope.Project);

                // Automatically extract all keywords from the Styles.
                provider.keywords = GetSearchKeywordsFromPath(AllPlatformSettings.AllPlatformSettingsPath);
                return provider;
            }

            // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
            return null;
        }

        #endregion
    }
}