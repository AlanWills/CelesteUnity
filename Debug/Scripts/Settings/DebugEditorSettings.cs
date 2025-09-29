using Celeste.Parameters;
using Celeste.Tools.Settings;
using UnityEngine;
using Celeste.Tools;

namespace Celeste.Debug.Settings
{
    [CreateAssetMenu(
        fileName = nameof(DebugEditorSettings), 
        menuName = CelesteMenuItemConstants.DEBUG_MENU_ITEM + "Debug Editor Settings",
        order = CelesteMenuItemConstants.DEBUG_MENU_ITEM_PRIORITY)]
    public class DebugEditorSettings : EditorSettings<DebugEditorSettings>
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/Editor/Data/";
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
                isDebugBuildValue = EditorOnly.MustFindAsset<BoolValue>("IsDebugBuild");
            }
        }
#endif
    }
}