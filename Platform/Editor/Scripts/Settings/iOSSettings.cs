using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Platform
{
    [CreateAssetMenu(fileName = "iOSSettings", menuName = "Celeste/Platform/iOS Settings")]
    public class iOSSettings : PlatformSettings
    {
        #region Properties and Fields

        public static iOSSettings Debug
        {
            get { return AllPlatformSettings.instance.iOSDebug; }
        }

        public static iOSSettings Release
        {
            get { return AllPlatformSettings.instance.iOSRelease; }
        }

        [SerializeField]
        private iOSBuildType runInXCodeAs = iOSBuildType.Debug;

        #endregion

        protected override void ApplyImpl()
        {
            EditorUserBuildSettings.iOSBuildConfigType = runInXCodeAs;

            PlayerSettings.stripEngineCode = false;
            PlayerSettings.iOS.buildNumber = Version.ToString();
            UnityEngine.Debug.LogFormat("iOS version is now: {0}", PlayerSettings.iOS.buildNumber);
        }
    }
}
