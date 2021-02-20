using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Platform
{
    [CreateAssetMenu(fileName = "HTML5Settings", menuName = "Celeste/Platform/HTML5 Settings")]
    public class HTML5Settings : PlatformSettings
    {
        #region Properties and Fields

        private const string DEBUG_PATH = "Assets/Platform/HTML5/Debug.asset";
        private const string RELEASE_PATH = "Assets/Platform/HTML5/Release.asset";

        private static HTML5Settings debug;
        public static HTML5Settings Debug
        {
            get
            {
                if (debug == null)
                {
                    debug = AssetDatabase.LoadAssetAtPath<HTML5Settings>(DEBUG_PATH);
                }

                return debug;
            }
        }

        private static HTML5Settings release;
        public static HTML5Settings Release
        {
            get
            {
                if (release == null)
                {
                    release = AssetDatabase.LoadAssetAtPath<HTML5Settings>(RELEASE_PATH);
                }

                return release;
            }
        }

        #endregion

        protected override void ApplyImpl()
        {
        }
    }
}
