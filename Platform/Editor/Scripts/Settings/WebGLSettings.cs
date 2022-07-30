using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem
{
    [CreateAssetMenu(fileName = nameof(WebGLSettings), menuName = "Celeste/Build System/WebGL Settings")]
    public class WebGLSettings : PlatformSettings
    {
        #region Properties and Fields

        private const string DEBUG_PATH = "Assets/Platform/HTML5/Debug.asset";
        private const string RELEASE_PATH = "Assets/Platform/HTML5/Release.asset";

        private static WebGLSettings debug;
        public static WebGLSettings Debug
        {
            get
            {
                if (debug == null)
                {
                    debug = AssetDatabase.LoadAssetAtPath<WebGLSettings>(DEBUG_PATH);
                }

                return debug;
            }
        }

        private static WebGLSettings release;
        public static WebGLSettings Release
        {
            get
            {
                if (release == null)
                {
                    release = AssetDatabase.LoadAssetAtPath<WebGLSettings>(RELEASE_PATH);
                }

                return release;
            }
        }

        #endregion

        public override void SetDefaultValues()
        {
        }

        protected override void ApplyImpl()
        {
        }
    }
}
