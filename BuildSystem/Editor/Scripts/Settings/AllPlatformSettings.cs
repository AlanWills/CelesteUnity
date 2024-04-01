using Celeste;
using Celeste.Tools.Settings;
using CelesteEditor.Tools;
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
        public AndroidSettings AndroidDebug => m_androidDebug;
        public AndroidSettings AndroidReleaseApk => m_androidReleaseApk;
        public AndroidSettings AndroidReleaseBundle => m_androidReleaseBundle;
        public WindowsSettings WindowsDebug => m_windowsDebug;
        public WindowsSettings WindowsRelease => m_windowsRelease;
        public WebGLSettings WebGLDebug => m_webGLDebug;
        public WebGLSettings WebGLRelease => m_webGLRelease;

        [SerializeField] private iOSSettings m_iOSDebug;
        [SerializeField] private iOSSettings m_iOSRelease;

        [SerializeField] private AndroidSettings m_androidDebug;
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
            m_windowsDebug = FindOrCreatePlatformSettingsAsset<WindowsSettings>(WindowsPlatformSettingsPath, "WindowsDebug");
            m_windowsRelease = FindOrCreatePlatformSettingsAsset<WindowsSettings>(WindowsPlatformSettingsPath, "WindowsRelease");
            EditorUtility.SetDirty(this);

            AppVersion appVersion = CreateVersionAsset(WindowsPlatformSettingsPath, "WindowsAppVersion");
            m_windowsDebug.Version = appVersion;
            m_windowsRelease.Version = appVersion;
        }

        public void CreateAndroidSettings()
        {
            m_androidDebug = FindOrCreatePlatformSettingsAsset<AndroidSettings>(AndroidPlatformSettingsPath, "AndroidDebug");
            m_androidReleaseApk = FindOrCreatePlatformSettingsAsset<AndroidSettings>(AndroidPlatformSettingsPath, "AndroidReleaseApk");
            m_androidReleaseBundle = FindOrCreatePlatformSettingsAsset<AndroidSettings>(AndroidPlatformSettingsPath, "AndroidReleaseBundle");
            EditorUtility.SetDirty(this);

            AppVersion appVersion = CreateVersionAsset(AndroidPlatformSettingsPath, "AndroidAppVersion");
            m_androidDebug.Version = appVersion;
            m_androidReleaseApk.Version = appVersion;
            m_androidReleaseBundle.Version = appVersion;
        }

        public void CreateiOSSettings()
        {
            m_iOSDebug = FindOrCreatePlatformSettingsAsset<iOSSettings>(iOSPlatformSettingsPath, "iOSDebug");
            m_iOSRelease = FindOrCreatePlatformSettingsAsset<iOSSettings>(iOSPlatformSettingsPath, "iOSRelease");
            EditorUtility.SetDirty(this);

            AppVersion appVersion = CreateVersionAsset(iOSPlatformSettingsPath, "iOSAppVersion");
            m_iOSDebug.Version = appVersion;
            m_iOSRelease.Version = appVersion;
        }

        public void CreateWebGLSettings()
        {
            m_webGLDebug = FindOrCreatePlatformSettingsAsset<WebGLSettings>(WebGLPlatformSettingsPath, "WebGLDebug");
            m_webGLRelease = FindOrCreatePlatformSettingsAsset<WebGLSettings>(WebGLPlatformSettingsPath, "WebGLRelease");
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

        public static T FindOrCreatePlatformSettingsAsset<T>(string folder, string settingsName) where T : PlatformSettings
        {
            T existingSettings = EditorOnly.FindAsset<T>(settingsName, folder);
            
            if (existingSettings  != null)
            {
                return existingSettings;
            }

            EditorOnly.CreateFolder(folder);

            T settings = CreateInstance<T>();
            settings.name = settingsName;
            settings.SetDefaultValues();

            EditorOnly.CreateAssetInFolderAndSave(settings, folder);

            return settings;
        }
    }
}