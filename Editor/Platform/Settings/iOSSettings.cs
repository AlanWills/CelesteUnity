using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace CelesteEditor.Platform
{
    [CreateAssetMenu(fileName = "iOSSettings", menuName = "Celeste/Platform/iOS Settings")]
    public class iOSSettings : PlatformSettings
    {
        #region Properties and Fields

        private const string DEBUG_PATH = "Assets/Platform/iOS/Debug.asset";
        private const string RELEASE_PATH = "Assets/Platform/iOS/Release.asset";

        private static iOSSettings debug;
        public static iOSSettings Debug
        {
            get
            {
                if (debug == null)
                {
                    debug = AssetDatabase.LoadAssetAtPath<iOSSettings>(DEBUG_PATH);
                }

                return debug;
            }
        }

        private static iOSSettings release;
        public static iOSSettings Release
        {
            get
            {
                if (release == null)
                {
                    release = AssetDatabase.LoadAssetAtPath<iOSSettings>(RELEASE_PATH);
                }

                return release;
            }
        }

        [SerializeField]
        private iOSBuildType runInXCodeAs = iOSBuildType.Debug;

        #endregion

        protected override void ApplyImpl()
        {
            EditorUserBuildSettings.iOSBuildConfigType = runInXCodeAs;

            PlayerSettings.stripEngineCode = false;
            PlayerSettings.iOS.buildNumber = Version;
            UnityEngine.Debug.LogFormat("iOS version is now: {0}", PlayerSettings.iOS.buildNumber);
        }
    }
}
