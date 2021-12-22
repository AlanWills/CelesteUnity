using System;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Platform
{
    [CreateAssetMenu(fileName = "AndroidSettings", menuName = "Celeste/Platform/Android Settings")]
    public class AndroidSettings : PlatformSettings
    {
        #region Properties and Fields

        public static AndroidSettings Debug
        {
            get { return AllPlatformSettings.instance.AndroidDebug; }
        }

        public static AndroidSettings ReleaseApk
        {
            get { return AllPlatformSettings.instance.AndroidReleaseApk; }
        }

        public static AndroidSettings ReleaseBundle
        {
            get { return AllPlatformSettings.instance.AndroidReleaseBundle; }
        }

        // HideInInspector cos enum not editable via default UI
        [SerializeField, HideInInspector]
        private ScriptingImplementation scriptingBackend;
        public ScriptingImplementation ScriptingBackend
        {
            get { return scriptingBackend; }
        }

        // HideInInspector cos enum not editable via default UI
        [SerializeField, HideInInspector]
        private AndroidArchitecture architecture;
        public AndroidArchitecture Architecture
        {
            get { return architecture; }
            set 
            { 
                architecture = value;
                EditorUtility.SetDirty(this);
            }
        }

        [SerializeField]
        [Tooltip("Whether the build pipeline should create an apk or aab.  Set this to true if building for store release, as only an aab can be uploading to Google Play.")]
        private bool buildAppBundle;
        private bool BuildAppBundle
        {
            get { return buildAppBundle; }
        }

        [SerializeField]
        [Tooltip("A flag to instruct the build pipeline to create debug symbols to help symbolicate crashes?")]
        private bool buildSymbols;
        private bool BuildSymbols
        {
            get { return buildSymbols; }
        }

        [SerializeField]
        [Tooltip("A flag to indicate if the build pipeline strip out unused code to reduce app size.")]
        private bool minifyRelease;
        public bool MinifyRelease
        {
            get { return minifyRelease; }
        }

        [SerializeField]
        [Tooltip("The password for the local keystore used to sign the build.")]
        private string keystorePassword;
        public string KeystorePassword
        {
            get { return keystorePassword; }
        }

        [SerializeField]
        [Tooltip("The key used to sign the build.")]
        private string keyAliasName;
        public string KeyAliasName
        {
            get { return keyAliasName; }
        }

        [SerializeField]
        [Tooltip("The password for the key used to sign the build.")]
        private string keyAliasPassword;
        public string KeyAliasPassword
        {
            get { return keyAliasPassword; }
        }

        #endregion

        protected override void ApplyImpl()
        {
            Version version = ParseVersion(Version);
            PlayerSettings.Android.bundleVersionCode = version.Major * 10000 + version.Minor * 100 + version.Build;
            UnityEngine.Debug.Log($"Android version is now: {version}");

            PlayerSettings.Android.keystorePass = KeystorePassword;
            PlayerSettings.Android.keyaliasName = KeyAliasName;
            PlayerSettings.Android.keyaliasPass = KeyAliasPassword;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingBackend);
            PlayerSettings.Android.targetArchitectures = Architecture;
            PlayerSettings.Android.minifyRelease = MinifyRelease;
            EditorUserBuildSettings.buildAppBundle = BuildAppBundle;
            EditorUserBuildSettings.androidCreateSymbolsZip = BuildSymbols;
        }
    }
}
