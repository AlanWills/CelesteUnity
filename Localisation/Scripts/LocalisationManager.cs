using Celeste.Localisation.Catalogue;
using Celeste.Localisation.Parameters;
using System.Globalization;
using UnityEngine;

namespace Celeste.Localisation
{
    [AddComponentMenu("Celeste/Localisation/Localisation Manager")]
    public class LocalisationManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private LanguageCatalogue languageCatalogue;
        [SerializeField] private LanguageValue currentLanguage;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            string currentTwoLetterISOCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            Language language = languageCatalogue.FindLanguageForTwoLetterCountryCode(currentTwoLetterISOCode);

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
