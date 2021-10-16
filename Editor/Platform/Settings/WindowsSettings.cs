using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace CelesteEditor.Platform
{
    [CreateAssetMenu(fileName = "WindowsSettings", menuName = "Celeste/Platform/Windows Settings")]
    public class WindowsSettings : PlatformSettings
    {
        #region Properties and Fields

        private const string DEBUG_PATH = "Assets/Platform/Windows/Debug.asset";
        private const string RELEASE_PATH = "Assets/Platform/Windows/Release.asset";

        private static WindowsSettings debug;
        public static WindowsSettings Debug
        {
            get
            {
                if (debug == null)
                {
                    debug = AssetDatabase.LoadAssetAtPath<WindowsSettings>(DEBUG_PATH);
                }

                return debug;
            }
        }

        private static WindowsSettings release;
        public static WindowsSettings Release
        {
            get
            {
                if (release == null)
                {
                    release = AssetDatabase.LoadAssetAtPath<WindowsSettings>(RELEASE_PATH);
                }

                return release;
            }
        }

        #endregion

        protected override void ApplyImpl()
        {
            EditorUserBuildSettings.selectedStandaloneTarget = BuildTarget;
        }

        protected override void InjectBuildEnvVars(StringBuilder stringBuilder)
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
