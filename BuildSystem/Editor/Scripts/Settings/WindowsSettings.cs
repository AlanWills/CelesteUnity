﻿using Celeste;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem
{
    [CreateAssetMenu(
        fileName = "WindowsSettings", 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Windows Settings",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class WindowsSettings : PlatformSettings
    {
        #region Properties and Fields

        [SerializeField, Header("Windows Settings")] private FullScreenMode fullScreenMode = FullScreenMode.FullScreenWindow;
        [SerializeField] private bool resizeableWindow = false;

        #endregion

        public override void SetDefaultValues()
        {
            BuildDirectory = "Builds/Windows/{version}";
            OutputName = "Build-{version}.exe";
            BuildTarget = BuildTarget.StandaloneWindows64;
            BuildTargetGroup = BuildTargetGroup.Standalone;
        }

        protected override void ApplyImpl()
        {
            EditorUserBuildSettings.selectedStandaloneTarget = BuildTarget;
            PlayerSettings.fullScreenMode = fullScreenMode;
            PlayerSettings.resizableWindow = resizeableWindow;
        }

        public override void InjectBuildEnvVars(StringBuilder stringBuilder)
        {
            base.InjectBuildEnvVars(stringBuilder);

            DirectoryInfo rootFolder = new DirectoryInfo(Application.dataPath).Parent;
            DirectoryInfo buildFolder = new DirectoryInfo(Path.Combine(BuildDirectory, OutputName)).Parent;

            stringBuilder.AppendLine();

            if (buildFolder.FullName.StartsWith(rootFolder.FullName))
            {
                // +1 for getting rid of \\ too
                stringBuilder.AppendFormat("BUILD_DIRECTORY={0}", buildFolder.FullName.Substring(rootFolder.FullName.Length + 1));
            }
            else
            {
                stringBuilder.AppendFormat("BUILD_DIRECTORY={0}", buildFolder.FullName);
            }
        }
    }
}
