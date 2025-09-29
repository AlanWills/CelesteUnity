using Celeste;
using Celeste.Tools.Settings;
using UnityEngine;

namespace CelesteEditor.Assets
{
    [CreateAssetMenu(
        fileName = nameof(AssetEditorSettings), 
        menuName = CelesteMenuItemConstants.ASSETS_MENU_ITEM + "Asset Editor Settings",
        order = CelesteMenuItemConstants.ASSETS_MENU_ITEM_PRIORITY)]
    public class AssetEditorSettings : EditorSettings<AssetEditorSettings>
    {
        #region Properties and Fields

        private const string FOLDER_PATH = "Assets/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "AssetsEditorSettings.asset";

        public bool showAddressableGroupSelection;
        public bool showExportAsJsonForScriptableObjects = true;
        public bool showApplyJsonForScriptableObjects = true;
        public bool showCopyGuidForObjects = true;

        #endregion

        public static AssetEditorSettings GetOrCreateSettings()
        {
            return GetOrCreateSettings(FOLDER_PATH, FILE_PATH);
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            
            // Unity 2022 seems to have this group selection UI by default now, so we don't show our own version 
#if UNITY_2022
            showAddressableGroupSelection = false;
#else
            showAddressableGroupSelection = true;
#endif
        }
    }
}