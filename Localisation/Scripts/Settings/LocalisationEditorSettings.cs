using Celeste.Localisation.Catalogue;
using Celeste.Localisation.Parameters;
using Celeste.Tools.Settings;
using UnityEngine;
using Celeste.Tools;

namespace Celeste.Localisation.Settings
{
    [CreateAssetMenu(fileName = nameof(LocalisationEditorSettings), menuName = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM + "Localisation Editor Settings", order = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM_PRIORITY)]
    public class LocalisationEditorSettings : EditorSettings<LocalisationEditorSettings>
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "LocalisationEditorSettings.asset";

        public LanguageValue currentLanguageValue;
        public LocalisationKeyCatalogue localisationKeyCatalogue;

        #endregion

#if UNITY_EDITOR
        public static LocalisationEditorSettings GetOrCreateSettings()
        {
            return GetOrCreateSettings(FOLDER_PATH, FILE_PATH);
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            if (currentLanguageValue == null)
            {
                currentLanguageValue = EditorOnly.MustFindAsset<LanguageValue>("CurrentLanguage");
            }
        }
#endif
    }
}