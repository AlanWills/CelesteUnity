using Celeste.Localisation.AssetReferences;
using Celeste.Localisation.Parameters;
using System.Collections;
using UnityEngine;

namespace Celeste.Localisation.Settings
{
    [CreateAssetMenu(fileName = nameof(LocalisationSettingsUsingAddressables), menuName = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM + "Localisation Settings Using Addressables", order = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM_PRIORITY)]
    public class LocalisationSettingsUsingAddressables : LocalisationSettings
    {
        #region Properties and Fields

        [SerializeField] private AssetReferenceLanguageCatalogue languageCatalogue;
        [SerializeField] private LanguageValue currentLanguage;
        [SerializeField] private Language fallbackLanguage;

        #endregion

        public override bool ShouldLoadAssets()
        {
            return languageCatalogue.ShouldLoad;
        }

        public override IEnumerator LoadAssets()
        {
            yield return languageCatalogue.LoadAssetAsync();
        }

        public override void SetCurrentLanguage(string languageCode)
        {
            Language language = languageCatalogue.Asset.FindLanguageForTwoLetterCountryCode(languageCode);
            UnityEngine.Debug.Assert(language != null, $"Could not find language for iso code {languageCode}.");
            
            SetCurrentLanguage(language);   
        }

        public override void SetCurrentLanguage(Language language)
        {
            if (language != null)
            {
                currentLanguage.Value = language;
            }
            else
            {
                UnityEngine.Debug.LogWarning($"Inputted language was null, so setting current language to fallback.");
                currentLanguage.Value = fallbackLanguage;
            }
        }
    }
}
