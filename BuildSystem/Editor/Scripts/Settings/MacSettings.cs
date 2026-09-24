using Celeste;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem
{
    [CreateAssetMenu(
        fileName = nameof(MacSettings), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Mac Settings",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class MacSettings : PlatformSettings
    {
        #region Properties and Fields

        [Header("Mac Settings")]
        [SerializeField] private FullScreenMode fullScreenMode = FullScreenMode.FullScreenWindow;
        [SerializeField] private bool resizeableWindow = false;

        private const string ApplicationSuffix = ".app";
        
        #endregion

        protected override void SetPlatformDefaultValues(bool isDebugConfig)
        {
            BuildTarget = BuildTarget.StandaloneOSX;
            BuildTargetGroup = BuildTargetGroup.Standalone;
            OutputName = $"Build-{{{VERSION_VARIABLE_NAME}}}-{{{ENVIRONMENT_VARIABLE_NAME}}}{ApplicationSuffix}";
        }

        protected override BuildPlayerOptions ModifyBuildPlayerOptions(BuildPlayerOptions buildPlayerOptions)
        {
            if (!buildPlayerOptions.locationPathName.EndsWith(ApplicationSuffix) &&
                !buildPlayerOptions.locationPathName.Contains('.'))
            {
                // Pretty crude test, but if we don't end in the application suffix and it doesn't look like we have an extension, we can add ours here
                buildPlayerOptions.locationPathName += ApplicationSuffix;
            }

            return buildPlayerOptions;
        }

        protected override void ApplyImpl()
        {
            EditorUserBuildSettings.selectedStandaloneTarget = BuildTarget;
            PlayerSettings.fullScreenMode = fullScreenMode;
            PlayerSettings.resizableWindow = resizeableWindow;
        }
    }
}
