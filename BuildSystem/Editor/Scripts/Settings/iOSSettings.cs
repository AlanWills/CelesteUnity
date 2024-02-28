﻿using Celeste;
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
            get { return runInXCodeAs; }
            set
            {
                runInXCodeAs = value;
                EditorUtility.SetDirty(this);
            }
        }

        #endregion

        public override void SetDefaultValues()
        {
            BuildDirectory = "Builds/iOS";
            OutputName = "Build-{version}";
            BuildTarget = BuildTarget.iOS;
            BuildTargetGroup = BuildTargetGroup.iOS;
            RunInXCodeAs = XcodeBuildConfig.Release;
        }

        protected override void ApplyImpl()
        {
            EditorUserBuildSettings.iOSXcodeBuildConfig = runInXCodeAs;

            PlayerSettings.stripEngineCode = false;
            PlayerSettings.iOS.buildNumber = Version.ToString();
            UnityEngine.Debug.LogFormat("iOS version is now: {0}", PlayerSettings.iOS.buildNumber);
        }
    }
}
