using Celeste.Assets;
using Celeste.Localisation.AssetReferences;
using Celeste.Localisation.Catalogue;
using Celeste.Localisation.Parameters;
using System.Collections;
using System.Globalization;
using UnityEngine;

namespace Celeste.Localisation
{
    [AddComponentMenu("Celeste/Localisation/Localisation Manager")]
    public class LocalisationManager : MonoBehaviour, IHasAssets
    {
        #region Properties and Fields

        [SerializeField] private AssetReferenceLanguageCatalogue languageCatalogue;
        [SerializeField] private LanguageValue currentLanguage;

        #endregion

        #region IHasAssets

        public bool ShouldLoadAssets()
        {
            return languageCatalogue.ShouldLoad();
        }

        public IEnumerator LoadAssets()
        {
            yield return languageCatalogue.LoadAssetAsync();

            string currentTwoLetterISOCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            Language language = languageCatalogue.Asset.FindLanguageForTwoLetterCountryCode(currentTwoLetterISOCode);
            language.Initialize();

            if (language != null)
            {
                currentLanguage.Value = language;
            }
            else
            {
                UnityEngine.Debug.LogWarning($"Could not find language for iso code {currentTwoLetterISOCode}.  Reverting to fallback.");
            }
        }

        #endregion

        #region Callbacks

        public void OnSetCurrentLanguage(Language newLanguage)
        {
            currentLanguage.Value = newLanguage;
        }

        #endregion
    }
}
