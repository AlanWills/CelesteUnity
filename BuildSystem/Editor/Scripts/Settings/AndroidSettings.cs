using Celeste;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem
{
    [CreateAssetMenu(
        fileName = "AndroidSettings", 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Android Settings",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class AndroidSettings : PlatformSettings
    {
        #region Properties and Fields

        [SerializeField, Header("Android Settings")]
        [Tooltip("Whether the build pipeline should create an apk or aab.  Set this to true if building for store release, as only an aab can be uploading to Google Play.")]
        private bool buildAppBundle;
        private bool BuildAppBundle => buildAppBundle;

        [SerializeField]
        [Tooltip("A flag to instruct the build pipeline to create debug symbols to help symbolicate crashes?")]
        private AndroidCreateSymbols buildSymbols = AndroidCreateSymbols.Disabled;
        private AndroidCreateSymbols BuildSymbols => buildSymbols;

        [SerializeField]
        [Tooltip("A flag to indicate if the build pipeline strip out unused code to reduce app size.")]
        private bool minifyRelease;
        public bool MinifyRelease => minifyRelease;

        [SerializeField]
        [Tooltip("The password for the local keystore used to sign the build.")]
        private string keystorePassword;
        public string KeystorePassword => keystorePassword;

        [SerializeField]
        [Tooltip("The key used to sign the build.")]
        private string keyAliasName;
        public string KeyAliasName => keyAliasName;

        [SerializeField]
        [Tooltip("The password for the key used to sign the build.")]
        private string keyAliasPassword;
        public string KeyAliasPassword => keyAliasPassword;

        [SerializeField]
        [Tooltip("If true, the permission to access the SD card for writing will be prompted to the user.")]
        private bool requiresWritePermission;
        public bool RequiresWritePermission
        {
            get => requiresWritePermission;
            set
            {
                if (requiresWritePermission != value)
                {
                    requiresWritePermission = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        [SerializeField]
        [Tooltip("The minimum Android Sdk version that this build will be able to target.")]
        private AndroidSdkVersions minSdkVersion = AndroidSdkVersions.AndroidApiLevel28;
        public AndroidSdkVersions MinSdkVersion
        {
            get => minSdkVersion;
            set
            {
                if (minSdkVersion != value)
                {
                    minSdkVersion = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        [SerializeField]
        [Tooltip("The desired Android Sdk version that this build will be able to target.")]
        private AndroidSdkVersions targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        public AndroidSdkVersions TargetSdkVersion
        {
            get => targetSdkVersion;
            set
            {
                if (targetSdkVersion != value)
                {
                    targetSdkVersion = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        // HideInInspector cos enum not editable via default UI
        [SerializeField]
        private ScriptingImplementation scriptingBackend;
        public ScriptingImplementation ScriptingBackend
        {
            get => scriptingBackend;
            set
            {
                if (scriptingBackend != value)
                {
                    scriptingBackend = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        [SerializeField]
        private AndroidArchitecture architecture;
        public AndroidArchitecture Architecture
        {
            get => architecture;
            set
            {
                if (architecture != value)
                {
                    architecture = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        public const int DEFAULT_TARGET_SDK_VERSION = 33;

        #endregion

        public void SetDefaultValues(bool isDebugConfig, bool isAppBundle)
        {
            SetDefaultValues(isDebugConfig);

            buildAppBundle = isAppBundle;

            string environmentSuffix = isAppBundle ? "Bundle" : "Apk";
            BuildDirectory = "Builds/{build_target}/{environment}" + environmentSuffix;

            // If building an app bundle we should include all architectures by default
            Architecture = isAppBundle ? AndroidArchitecture.All : Architecture;

            // Build symbols for release bundles only by default
            buildSymbols = isDebugConfig || isAppBundle == false ? AndroidCreateSymbols.Disabled : AndroidCreateSymbols.Public;
        }

        protected override void SetPlatformDefaultValues(bool isDebugConfig)
        {
            BuildTarget = BuildTarget.Android;
            BuildTargetGroup = BuildTargetGroup.Android;
            ScriptingBackend = ScriptingImplementation.IL2CPP;
            Architecture = AndroidArchitecture.ARMv7;
            RequiresWritePermission = true;
            MinSdkVersion = AndroidSdkVersions.AndroidApiLevel28;
            TargetSdkVersion = (AndroidSdkVersions)DEFAULT_TARGET_SDK_VERSION;
        }

        protected override BuildPlayerOptions ModifyBuildPlayerOptions(BuildPlayerOptions buildPlayerOptions)
        {
            string desiredExtension = buildAppBundle ? ".aab" : ".apk";
            
            if (!buildPlayerOptions.locationPathName.EndsWith(desiredExtension))
            {
                // Pretty crude test, but if we don't end in the correct extension and it doesn't look like we have an extension, we can add ours here
                buildPlayerOptions.locationPathName += desiredExtension;
            }

            return buildPlayerOptions;
        }

        protected override void ApplyImpl()
        {
            PlayerSettings.Android.bundleVersionCode = Version.Major * 10000 + Version.Minor * 100 + Version.Build;
            Debug.Log($"Android version is now: {Version}");

            PlayerSettings.Android.keystorePass = KeystorePassword;
            PlayerSettings.Android.keyaliasName = KeyAliasName;
            PlayerSettings.Android.keyaliasPass = KeyAliasPassword;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingBackend);
            PlayerSettings.Android.targetArchitectures = Architecture;
            PlayerSettings.Android.minifyRelease = MinifyRelease;
            EditorUserBuildSettings.buildAppBundle = BuildAppBundle;
            EditorUserBuildSettings.androidCreateSymbols = BuildSymbols;
            PlayerSettings.Android.forceSDCardPermission = RequiresWritePermission;
            PlayerSettings.Android.minSdkVersion = MinSdkVersion;
            PlayerSettings.Android.targetSdkVersion = TargetSdkVersion;
        }
    }
}
