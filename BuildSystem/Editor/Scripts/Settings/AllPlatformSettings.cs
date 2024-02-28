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
            m_windowsDebug = CreateSettingsAsset<WindowsSettings>(WindowsPlatformSettingsPath, "WindowsDebug");
            m_windowsRelease = CreateSettingsAsset<WindowsSettings>(WindowsPlatformSettingsPath, "WindowsRelease");
            EditorUtility.SetDirty(this);

            AppVersion appVersion = CreateVersionAsset(WindowsPlatformSettingsPath, "WindowsAppVersion");
            m_windowsDebug.Version = appVersion;
            m_windowsRelease.Version = appVersion;
        }

        public void CreateAndroidSettings()
        {
            m_androidDebug = CreateSettingsAsset<AndroidSettings>(AndroidPlatformSettingsPath, "AndroidDebug");
            m_androidReleaseApk = CreateSettingsAsset<AndroidSettings>(AndroidPlatformSettingsPath, "AndroidReleaseApk");
            m_androidReleaseBundle = CreateSettingsAsset<AndroidSettings>(AndroidPlatformSettingsPath, "AndroidReleaseBundle");
            EditorUtility.SetDirty(this);

            AppVersion appVersion = CreateVersionAsset(AndroidPlatformSettingsPath, "AndroidAppVersion");
            m_androidDebug.Version = appVersion;
            m_androidReleaseApk.Version = appVersion;
            m_androidReleaseBundle.Version = appVersion;
        }

        public void CreateiOSSettings()
        {
            m_iOSDebug = CreateSettingsAsset<iOSSettings>(iOSPlatformSettingsPath, "iOSDebug");
            m_iOSRelease = CreateSettingsAsset<iOSSettings>(iOSPlatformSettingsPath, "iOSRelease");
            EditorUtility.SetDirty(this);

            AppVersion appVersion = CreateVersionAsset(iOSPlatformSettingsPath, "iOSAppVersion");
            m_iOSDebug.Version = appVersion;
            m_iOSRelease.Version = appVersion;
        }

        public void CreateWebGLSettings()
        {
            m_webGLDebug = CreateSettingsAsset<WebGLSettings>(WebGLPlatformSettingsPath, "WebGLDebug");
            m_webGLRelease = CreateSettingsAsset<WebGLSettings>(WebGLPlatformSettingsPath, "WebGLRelease");
            EditorUtility.SetDirty(this);

            AppVersion appVersion = CreateVersionAsset(WebGLPlatformSettingsPath, "WebGLAppVersion");
            m_webGLDebug.Version = appVersion;
            m_webGLRelease.Version = appVersion;
        }

        private AppVersion CreateVersionAsset(string folder, string versionName)
        {
            AssetUtility.CreateFolder(folder);

            AppVersion appVersion = CreateInstance<AppVersion>();
            appVersion.name = versionName;

            AssetUtility.CreateAssetInFolderAndSave(appVersion, folder);

            return appVersion;
        }

        public T CreateSettingsAsset<T>(string folder, string settingsName) where T : PlatformSettings
        {
            AssetUtility.CreateFolder(folder);

            T settings = CreateInstance<T>();
            settings.name = settingsName;
            settings.SetDefaultValues();

            AssetUtility.CreateAssetInFolderAndSave(settings, folder);

            return settings;
        }
    }
}