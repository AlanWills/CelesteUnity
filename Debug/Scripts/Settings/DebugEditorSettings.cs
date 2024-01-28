using Celeste.Parameters;
using Celeste.Tools.Settings;
using UnityEngine;
#if UNITY_EDITOR
using CelesteEditor.Tools;
#endif

namespace Celeste.Debug.Settings
{
    [CreateAssetMenu(fileName = nameof(DebugEditorSettings), menuName = "Celeste/Debug/Debug Editor Settings")]
    public class DebugEditorSettings : EditorSettings<DebugEditorSettings>
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/Debug/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "DebugEditorSettings.asset";

        public BoolValue isDebugBuildValue;

        #endregion

#if UNITY_EDITOR
        public static DebugEditorSettings GetOrCreateSettings()
        {
            return GetOrCreateSettings(FOLDER_PATH, FILE_PATH);
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            if (isDebugBuildValue == null)
            {
                isDebugBuildValue = AssetUtility.FindAsset<BoolValue>("IsDebugBuild");
            }
        }
#endif
    }
}