using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Platform
{
    [CreateAssetMenu(fileName = "iOSSettings", menuName = "Celeste/Platform/iOS Settings")]
    public class iOSSettings : PlatformSettings
    {
        #region Properties and Fields

        [SerializeField]
        private iOSBuildType runInXCodeAs = iOSBuildType.Debug;
        public iOSBuildType RunInXCodeAs
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
            RunInXCodeAs = iOSBuildType.Release;
        }

        protected override void ApplyImpl()
        {
            EditorUserBuildSettings.iOSBuildConfigType = runInXCodeAs;

            PlayerSettings.stripEngineCode = false;
            PlayerSettings.iOS.buildNumber = Version.ToString();
            UnityEngine.Debug.LogFormat("iOS version is now: {0}", PlayerSettings.iOS.buildNumber);
        }
    }
}
