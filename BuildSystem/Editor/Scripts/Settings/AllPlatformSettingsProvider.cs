﻿using Celeste;
using CelesteEditor.Tools;
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
        private SerializedProperty androidDebugProperty;
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
            androidDebugProperty = allPlatformSettings.FindProperty("m_androidDebug");
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
                SettingsGUI<iOSSettings>(
                    iOSDebugProperty,
                    "iOS Debug",
                    AllPlatformSettings.iOSPlatformSettingsPath,
                    "iOSDebug",
                    true);

                SettingsGUI<iOSSettings>(
                    iOSReleaseProperty,
                    "iOS Release",
                    AllPlatformSettings.iOSPlatformSettingsPath,
                    "iOSRelease",
                    false);
            }
        }

        private void DrawAndroidSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Android", CelesteGUIStyles.BoldLabel);
            EditorGUILayout.Space();

            using (EditorGUI.IndentLevelScope indent = new EditorGUI.IndentLevelScope())
            {
                SettingsGUI<AndroidSettings>(
                    androidDebugProperty,
                    "Android Debug",
                    AllPlatformSettings.AndroidPlatformSettingsPath,
                    "AndroidDebug",
                    true);

                SettingsGUI<AndroidSettings>(
                    androidReleaseApkProperty,
                    "Android Release Apk",
                    AllPlatformSettings.AndroidPlatformSettingsPath,
                    "AndroidReleaseApk",
                    false);

                SettingsGUI<AndroidSettings>(
                    androidReleaseBundleProperty,
                    "Android Release Bundle",
                    AllPlatformSettings.AndroidPlatformSettingsPath,
                    "AndroidReleaseBundle",
                    false);
            }
        }

        private void DrawWindowsSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Windows", CelesteGUIStyles.BoldLabel);
            EditorGUILayout.Space();

            using (EditorGUI.IndentLevelScope indent = new EditorGUI.IndentLevelScope())
            {
                SettingsGUI<WindowsSettings>(
                    windowsDebugProperty,
                    "Windows Debug",
                    AllPlatformSettings.WindowsPlatformSettingsPath,
                    "WindowsDebug",
                    true);

                SettingsGUI<WindowsSettings>(
                    windowsReleaseProperty,
                    "Windows Release",
                    AllPlatformSettings.WindowsPlatformSettingsPath,
                    "WindowsRelease",
                    false);
            }
        }

        private void DrawWebGLSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("WebGL", CelesteGUIStyles.BoldLabel);
            EditorGUILayout.Space();

            using (EditorGUI.IndentLevelScope indent = new EditorGUI.IndentLevelScope())
            {
                SettingsGUI<WebGLSettings>(
                    webGLDebugProperty,
                    "WebGL Debug",
                    AllPlatformSettings.WebGLPlatformSettingsPath,
                    "WebGLDebug",
                    true);

                SettingsGUI<WebGLSettings>(
                    webGLReleaseProperty,
                    "WebGL Release",
                    AllPlatformSettings.WebGLPlatformSettingsPath,
                    "WebGLRelease",
                    false);
            }
        }

        private void SettingsGUI<T>(
            SerializedProperty settingsProperty,
            string settingsPropertyName,
            string folder,
            string name,
            bool isDebugConfig) where T : PlatformSettings
        {
            if (settingsProperty.objectReferenceValue != null)
            {
                EditorGUILayout.PropertyField(settingsProperty, new GUIContent(settingsPropertyName));
            }
            else if (GUILayout.Button($"Find Or Create {settingsPropertyName} Settings", GUILayout.ExpandWidth(false)))
            {
                FindOrCreateSettingsAsset<T>(
                    folder,
                    name,
                    settingsProperty,
                    isDebugConfig);
            }
        }

        private void FindOrCreateSettingsAsset<T>(
            string folder,
            string settingsName,
            SerializedProperty settingsProperty,
            bool isDebugConfig) where T : PlatformSettings
        {
            settingsProperty.objectReferenceValue = AllPlatformSettings.FindOrCreatePlatformSettingsAsset<T>(folder, settingsName, isDebugConfig);
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