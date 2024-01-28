using Celeste.Localisation.Catalogue;
using Celeste.Localisation.Parameters;
using Celeste.Tools.Settings;
using UnityEngine;
#if UNITY_EDITOR
using CelesteEditor.Tools;
#endif

namespace Celeste.Localisation.Settings
{
    [CreateAssetMenu(fileName = nameof(LocalisationEditorSettings), menuName = "Celeste/Localisation/Localisation Editor Settings")]
    public class LocalisationEditorSettings : EditorSettings<LocalisationEditorSettings>
    {
        #region Properties and Fields

        public const string FOLDER_PATH = "Assets/Localisation/Editor/Data/";
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
                currentLanguageValue = AssetUtility.FindAsset<LanguageValue>("CurrentLanguage");
            }
        }
#endif
    }
}