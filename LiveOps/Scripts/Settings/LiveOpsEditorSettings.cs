﻿using Celeste.Components.Catalogue;
using Celeste.Tools.Settings;
using UnityEngine;

namespace Celeste.LiveOps.Settings
{
    [CreateAssetMenu(fileName = nameof(LiveOpsEditorSettings), menuName = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM + "Live Ops Editor Settings", order = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM_PRIORITY)]
    public class LiveOpsEditorSettings : EditorSettings<LiveOpsEditorSettings>
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/LiveOps/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "LiveOpsEditorSettings.asset";

        public ComponentCatalogue defaultComponentCatalogue;

        #endregion

#if UNITY_EDITOR
        public static LiveOpsEditorSettings GetOrCreateSettings()
        {
            return GetOrCreateSettings(FOLDER_PATH, FILE_PATH);
        }
#endif
    }
}