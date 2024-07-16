using Celeste;
using Celeste.Tools;
using Celeste.Tools.Settings;
using System;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem
{
    [CreateAssetMenu(
        fileName = nameof(AllPlatformSettings), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "All Platform Settings",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class AllPlatformSettings : EditorSettings<AllPlatformSettings>
    {
        #region Properties and Fields

        public iOSSettings iOSDebug => m_iOSDebug;
        public iOSSettings iOSRelease => m_iOSRelease;
        public AndroidSettings AndroidDebugApk => m_androidDebugApk;
        public AndroidSettings AndroidDebugBundle => m_androidDebugBundle;
        public AndroidSettings AndroidReleaseApk => m_androidReleaseApk;
        public AndroidSettings AndroidReleaseBundle => m_androidReleaseBundle;
        public WindowsSettings WindowsDebug => m_windowsDebug;
        public WindowsSettings WindowsRelease => m_windowsRelease;
        public WebGLSettings WebGLDebug => m_webGLDebug;
        public WebGLSettings WebGLRelease => m_webGLRelease;

        [SerializeField] private iOSSettings m_iOSDebug;
        [SerializeField] private iOSSettings m_iOSRelease;

        [SerializeField] private AndroidSettings m_androidDebugApk;
        [SerializeField] private AndroidSettings m_androidDebugBundle;
        [SerializeField] private AndroidSettings m_androidReleaseApk;
        [SerializeField] private AndroidSettings m_androidReleaseBundle;

        [SerializeField] private WindowsSettings m_windowsDebug;
        [SerializeField] private WindowsSettings m_windowsRelease;

        [SerializeField] private WebGLSettings m_webGLDebug;
        [SerializeField] private WebGLSettings m_webGLRelease;

        public const string AllPlatformSettingsDirectory = "Assets/BuildSystem/Editor/";
        public const string AllPlatformSettingsPath = AllPlatformSettingsDirectory + "AllPlatformSettings.asset";
        public const string iOSPlatformSettingsPath = AllPlatformSettingsDirectory + "iOS";
        public const string AndroidPlatformSettingsPath = AllPlatformSettingsDirectory + "Android";
        public const string WindowsPlatformSettingsPath = AllPlatformSettingsDirectory + "Windows";
        public const string WebGLPlatformSettingsPath = AllPlatformSettingsDirectory + "WebGL";

        #endregion

        public static AllPlatformSettings GetOrCreateSettings()
        {
            return GetOrCreateSettings(AllPlatformSettingsDirectory, AllPlatformSettingsPath);
        }

        public static SerializedObject GetSerializedSettings()
        {
            return GetSerializedSettings(AllPlatformSettingsDirectory, AllPlatformSettingsPath);
        }

        public void CreateWindowsSettings()
        {
            m_windowsDebug = FindOrCreateWindowsSettingsAsset(WindowsPlatformSettingsPath, "WindowsDebug", true);
            m_windowsRelease = FindOrCreateWindowsSettingsAsset(WindowsPlatformSettingsPath, "WindowsRelease", false);
            EditorUtility.SetDirty(this);

            AppVersion appVersion = CreateVersionAsset(WindowsPlatformSettingsPath, "WindowsAppVersion");
            m_windowsDebug.Version = appVersion;
            m_windowsRelease.Version = appVersion;
        }

        public void CreateAndroidSettings()
        {
            m_androidDebugApk = FindOrCreateAndroidSettingsAsset(AndroidPlatformSettingsPath, "AndroidDebugApk", true, false);
            m_androidDebugBundle = FindOrCreateAndroidSettingsAsset(AndroidPlatformSettingsPath, "AndroidDebugBundle", true, true);
            m_androidReleaseApk = FindOrCreateAndroidSettingsAsset(AndroidPlatformSettingsPath, "AndroidReleaseApk", false, false);
            m_androidReleaseBundle = FindOrCreateAndroidSettingsAsset(AndroidPlatformSettingsPath, "AndroidReleaseBundle", false, true);
            EditorUtility.SetDirty(this);

            AppVersion appVersion = CreateVersionAsset(AndroidPlatformSettingsPath, "AndroidAppVersion");
            m_androidDebugApk.Version = appVersion;
            m_androidReleaseApk.Version = appVersion;
            m_androidReleaseBundle.Version = appVersion;
        }

        public void CreateiOSSettings()
        {
            m_iOSDebug = FindOrCreateiOSSettingsAsset(iOSPlatformSettingsPath, "iOSDebug", true);
            m_iOSRelease = FindOrCreateiOSSettingsAsset(iOSPlatformSettingsPath, "iOSRelease", false);
            EditorUtility.SetDirty(this);

            AppVersion appVersion = CreateVersionAsset(iOSPlatformSettingsPath, "iOSAppVersion");
            m_iOSDebug.Version = appVersion;
            m_iOSRelease.Version = appVersion;
        }

        public void CreateWebGLSettings()
        {
            m_webGLDebug = FindOrCreateWebGLSettingsAsset(WebGLPlatformSettingsPath, "WebGLDebug", true);
            m_webGLRelease = FindOrCreateWebGLSettingsAsset(WebGLPlatformSettingsPath, "WebGLRelease", false);
            EditorUtility.SetDirty(this);

            AppVersion appVersion = CreateVersionAsset(WebGLPlatformSettingsPath, "WebGLAppVersion");
            m_webGLDebug.Version = appVersion;
            m_webGLRelease.Version = appVersion;
        }

        private AppVersion CreateVersionAsset(string folder, string versionName)
        {
            EditorOnly.CreateFolder(folder);

            AppVersion appVersion = CreateInstance<AppVersion>();
            appVersion.name = versionName;

            EditorOnly.CreateAssetInFolderAndSave(appVersion, folder);

            return appVersion;
        }

        public static WindowsSettings FindOrCreateWindowsSettingsAsset(
            string folder,
            string settingsName,
            bool isDebugConfig)
        {
            return FindOrCreatePlatformSettingsAsset<WindowsSettings>(
                folder, 
                settingsName, 
                s => s.SetDefaultValues(isDebugConfig));
        }

        public static AndroidSettings FindOrCreateAndroidSettingsAsset(
            string folder,
            string settingsName,
            bool isDebugConfig,
            bool isAppBundle)
        {
            return FindOrCreatePlatformSettingsAsset<AndroidSettings>(
                folder, 
                settingsName, 
                s => s.SetDefaultValues(isDebugConfig, isAppBundle));
        }

        public static iOSSettings FindOrCreateiOSSettingsAsset(
            string folder,
            string settingsName,
            bool isDebugConfig)
        {
            return FindOrCreatePlatformSettingsAsset<iOSSettings>(
                folder, 
                settingsName, 
                s => s.SetDefaultValues(isDebugConfig));
        }

        public static WebGLSettings FindOrCreateWebGLSettingsAsset(
            string folder,
            string settingsName,
            bool isDebugConfig)
        {
            return FindOrCreatePlatformSettingsAsset<WebGLSettings>(
                folder, 
                settingsName, 
                s => s.SetDefaultValues(isDebugConfig));
        }

        private static T FindOrCreatePlatformSettingsAsset<T>(
            string folder, 
            string settingsName, 
            Action<T> setDefaultValues) where T : PlatformSettings
        {
            T existingSettings = EditorOnly.FindAsset<T>(settingsName, folder);
            
            if (existingSettings  != null)
            {
                return existingSettings;
            }

            EditorOnly.CreateFolder(folder);

            T settings = CreateInstance<T>();
            settings.name = settingsName;
            setDefaultValues?.Invoke(settings);

            EditorOnly.CreateAssetInFolderAndSave(settings, folder);

            return existingSettings;
        }
    }
}