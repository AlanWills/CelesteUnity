using Celeste.Tools.Settings;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem
{
    [CreateAssetMenu(fileName = nameof(AllPlatformSettings), menuName = "Celeste/Build System/All Platform Settings")]
    public class AllPlatformSettings : EditorSettings<AllPlatformSettings>
    {
        #region Properties and Fields

        public iOSSettings iOSDebug
        {
            get { return m_iOSDebug; }
        }

        public iOSSettings iOSRelease
        {
            get { return m_iOSRelease; }
        }

        public AndroidSettings AndroidDebug
        {
            get { return m_androidDebug; }
        }

        public AndroidSettings AndroidReleaseApk
        {
            get { return m_androidReleaseApk; }
        }

        public AndroidSettings AndroidReleaseBundle
        {
            get { return m_androidReleaseBundle; }
        }

        public WindowsSettings WindowsDebug
        {
            get { return m_windowsDebug; }
        }

        public WindowsSettings WindowsRelease
        {
            get { return m_windowsRelease; }
        }

        public WebGLSettings WebGLDebug
        {
            get { return m_webGLDebug; }
        }

        public WebGLSettings WebGLRelease
        {
            get { return m_webGLRelease; }
        }

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
    }
}