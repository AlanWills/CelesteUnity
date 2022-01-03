using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CelesteEditor.Platform
{
    [FilePath(AllPlatformSettingsPath, FilePathAttribute.Location.ProjectFolder)]
    [CreateAssetMenu(fileName = nameof(AllPlatformSettings), menuName = "Celeste/Platform/All Platform Settings")]
    public class AllPlatformSettings : ScriptableSingleton<AllPlatformSettings>
    {
        #region Proeprties and Fields

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

        [SerializeField] private iOSSettings m_iOSDebug;
        [SerializeField] private iOSSettings m_iOSRelease;

        [SerializeField] private AndroidSettings m_androidDebug;
        [SerializeField] private AndroidSettings m_androidReleaseApk;
        [SerializeField] private AndroidSettings m_androidReleaseBundle;

        [SerializeField] private WindowsSettings m_windowsDebug;
        [SerializeField] private WindowsSettings m_windowsRelease;

        public const string AllPlatformSettingsDirectory = "Assets/Platform/Editor/";
        public const string AllPlatformSettingsPath = AllPlatformSettingsDirectory + "AllPlatformSettings.asset";
        public const string iOSPlatformSettingsPath = AllPlatformSettingsDirectory + "iOS";
        public const string AndroidPlatformSettingsPath = AllPlatformSettingsDirectory + "Android";
        public const string WindowsPlatformSettingsPath = AllPlatformSettingsDirectory + "Windows";

        #endregion
    }
}