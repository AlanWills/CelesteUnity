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
        #region Styles

        private class PlatformSettingStyles
        {
            public static GUIContent iOSDebug = new GUIContent("iOS Debug");
            public static GUIContent iOSRelease = new GUIContent("iOS Release");
            public static GUIContent AndroidDebug = new GUIContent("Android Debug");
            public static GUIContent AndroidReleaseApk = new GUIContent("Android Release Apk");
            public static GUIContent AndroidReleaseBundle = new GUIContent("Android Release Bundle");
            public static GUIContent WindowsDebugBundle = new GUIContent("Windows Debug Bundle");
            public static GUIContent WindowsReleaseBundle = new GUIContent("Windows Release Bundle");
            public static GUIContent WebGLDebugBundle = new GUIContent("WebGL Debug Bundle");
            public static GUIContent WebGLReleaseBundle = new GUIContent("WebGL Release Bundle");
        }

        #endregion

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

            DrawiOSSettings();
            DrawAndroidSettings();
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
                    PlatformSettingStyles.iOSDebug,
                    new GUIContent("Create iOS Debug Settings"),
                    AllPlatformSettings.iOSPlatformSettingsPath,
                    "iOSDebug");

                SettingsGUI<iOSSettings>(
                    iOSReleaseProperty,
                    PlatformSettingStyles.iOSRelease,
                    new GUIContent("Create iOS Release Settings"),
                    AllPlatformSettings.iOSPlatformSettingsPath,
                    "iOSRelease");
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
                    PlatformSettingStyles.AndroidDebug,
                    new GUIContent("Create Android Debug Settings"),
                    AllPlatformSettings.AndroidPlatformSettingsPath,
                    "AndroidDebug");

                SettingsGUI<AndroidSettings>(
                    androidReleaseApkProperty,
                    PlatformSettingStyles.AndroidReleaseApk,
                    new GUIContent("Create Android Release Apk Settings"),
                    AllPlatformSettings.AndroidPlatformSettingsPath,
                    "AndroidReleaseApk");

                SettingsGUI<AndroidSettings>(
                    androidReleaseBundleProperty,
                    PlatformSettingStyles.AndroidReleaseBundle,
                    new GUIContent("Create Android Release Bundle Settings"),
                    AllPlatformSettings.AndroidPlatformSettingsPath,
                    "AndroidReleaseBundle");
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
                    PlatformSettingStyles.WindowsDebugBundle,
                    new GUIContent("Create Windows Debug Settings"),
                    AllPlatformSettings.WindowsPlatformSettingsPath,
                    "WindowsDebug");

                SettingsGUI<WindowsSettings>(
                    windowsReleaseProperty,
                    PlatformSettingStyles.WindowsReleaseBundle,
                    new GUIContent("Create Windows Release Settings"),
                    AllPlatformSettings.WindowsPlatformSettingsPath,
                    "WindowsRelease");
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
                    PlatformSettingStyles.WebGLDebugBundle,
                    new GUIContent("Create WebGL Debug Settings"),
                    AllPlatformSettings.WebGLPlatformSettingsPath,
                    "WebGLDebug");

                SettingsGUI<WebGLSettings>(
                    webGLReleaseProperty,
                    PlatformSettingStyles.WebGLReleaseBundle,
                    new GUIContent("Create WebGL Release Settings"),
                    AllPlatformSettings.WebGLPlatformSettingsPath,
                    "WebGLRelease");
            }
        }

        private void SettingsGUI<T>(
            SerializedProperty settingsProperty,
            GUIContent settingsPropertyStyle,
            GUIContent createSettingsStyle,
            string folder,
            string name) where T : PlatformSettings
        {
            if (settingsProperty.objectReferenceValue != null)
            {
                EditorGUILayout.PropertyField(settingsProperty, settingsPropertyStyle);
            }
            else if (GUILayout.Button(createSettingsStyle, GUILayout.ExpandWidth(false)))
            {
                CreateSettingsAsset<T>(
                    folder,
                    name,
                    settingsProperty);
            }
        }

        private void CreateSettingsAsset<T>(
            string folder,
            string settingsName,
            SerializedProperty settingsProperty) where T : PlatformSettings
        {
            AssetUtility.CreateFolder(folder);

            T settings = ScriptableObject.CreateInstance<T>();
            settings.name = settingsName;
            settings.SetDefaultValues();

            AssetUtility.CreateAssetInFolderAndSave(settings, folder);

            settingsProperty.objectReferenceValue = AllPlatformSettings.GetOrCreateSettings().CreateSettingsAsset<T>(folder, settingsName);
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
                var provider = new AllPlatformSettingsProvider("Project/Celeste/Platform Settings", SettingsScope.Project);

                // Automatically extract all keywords from the Styles.
                provider.keywords = GetSearchKeywordsFromGUIContentProperties<PlatformSettingStyles>();
                return provider;
            }

            // Settings Asset doesn't exist yet; no need to display anything in the Settings window.
            return null;
        }

        #endregion
    }
}