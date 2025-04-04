using Celeste;
using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem
{
    [CreateAssetMenu(
        fileName = "iOSSettings", 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "iOS Settings",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class iOSSettings : PlatformSettings
    {
        #region Properties and Fields

        [SerializeField, Header("iOS Settings")]
        private XcodeBuildConfig runInXCodeAs = XcodeBuildConfig.Debug;
        public XcodeBuildConfig RunInXCodeAs
        {
            get => runInXCodeAs;
            set
            {
                if (runInXCodeAs != value)
                {
                    runInXCodeAs = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        [SerializeField, Tooltip("The .ipa extension will be added at runtime.")] private string ipaName;

        [Header("Custom Build System Settings")]
        [SerializeField] private bool useiOSProjectBuilder = true;
        [SerializeField, ShowIf(nameof(useiOSProjectBuilder))] private string iOSProjectBuilderInstallPath = "C:/Users/alawi/iOS Project Builder for Unity";
        [SerializeField, ShowIf(nameof(useiOSProjectBuilder)), Tooltip("A recommended setting")] private bool useTempDirectoryDuringBuild = true;
        [SerializeField, ShowIfAll(nameof(useiOSProjectBuilder), nameof(useTempDirectoryDuringBuild))] private string tempDirectoryPath = "C:/iOS";

        [Header("Credentials")]
        [SerializeField] private string distributionCertificate = "ios_distribution.cer";
        [SerializeField] private string privateKeyName = "";
        [SerializeField] private string privateKeyPassword = "";
        [SerializeField] private string provisioningProfileName = "";
        [SerializeField] private string appStoreConnectUsername = "";
        [SerializeField] private string appStoreConnectPassword = "";

        [Title("Distribution Settings")]
        [SerializeField] private bool useFastlane;
        public bool UseFastlane
        {
            get => useFastlane;
            set
            {
                if (useFastlane != value)
                {
                    useFastlane = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }

        #endregion

        protected override void SetPlatformDefaultValues(bool isDebugConfig)
        {
            BuildTarget = BuildTarget.iOS;
            BuildTargetGroup = BuildTargetGroup.iOS;
            RunInXCodeAs = XcodeBuildConfig.Release;
            useiOSProjectBuilder = true;
            iOSProjectBuilderInstallPath = "C:/Users/alawi/iOS Project Builder for Unity";

#if UNITY_EDITOR_WIN
            useTempDirectoryDuringBuild = true;
            tempDirectoryPath = "C:/iOS";
#else
            useTempDirectoryDuringBuild = false;
            tempDirectoryPath = string.Empty;
#endif
            UseFastlane = false;
        }

        protected override void ApplyImpl()
        {
            EditorUserBuildSettings.iOSXcodeBuildConfig = runInXCodeAs;

            PlayerSettings.stripEngineCode = false;
            PlayerSettings.iOS.buildNumber = Version.ToString();
            UnityEngine.Debug.Log($"iOS version is now: {PlayerSettings.iOS.buildNumber}.");
        }

        protected override void DoInjectBuildEnvVars(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine($"IPA_NAME={ipaName}");

            if (useiOSProjectBuilder)
            {
                
                stringBuilder.AppendLine($"USE_IOS_PROJECT_BUILDER={useiOSProjectBuilder}");
                stringBuilder.AppendLine($"IOS_PROJECT_BUILDER_INSTALL_PATH={EditorOnly.EnsureDelimitersCorrect(iOSProjectBuilderInstallPath)}");
                
                if(useTempDirectoryDuringBuild)
                {
                    stringBuilder.AppendLine($"USE_TEMP_DIRECTORY={useTempDirectoryDuringBuild}");
                    stringBuilder.AppendLine($"TEMP_DIRECTORY_PATH={EditorOnly.EnsureDelimitersCorrect(tempDirectoryPath)}");
                }
            }

            stringBuilder.AppendLine($"DISTRIBUTION_CERTIFICATE={distributionCertificate}");
            stringBuilder.AppendLine($"PRIVATE_KEY={privateKeyName}");
            stringBuilder.AppendLine($"PRIVATE_KEY_PASSWORD={privateKeyPassword}");
            stringBuilder.AppendLine($"PROVISIONING_PROFILE={provisioningProfileName}");
            stringBuilder.AppendLine($"ASC_USERNAME={appStoreConnectUsername}");
            stringBuilder.AppendLine($"ASC_PASSWORD={appStoreConnectPassword}");
            stringBuilder.AppendLine($"USE_FASTLANE={UseFastlane}");
        }
    }
}
