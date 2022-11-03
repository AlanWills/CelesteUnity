using Celeste.Assets;
using Celeste.Localisation.Settings;
using System.Collections;
using System.Globalization;
using UnityEngine;

namespace Celeste.Localisation
{
    [AddComponentMenu("Celeste/Localisation/Localisation Manager")]
    public class LocalisationManager : MonoBehaviour, IHasAssets
    {
        #region Properties and Fields

        [SerializeField] private LocalisationSettings localisationSettings;

        #endregion

        #region IHasAssets

        public bool ShouldLoadAssets()
        {
            return localisationSettings.ShouldLoadAssets();
        }

        public IEnumerator LoadAssets()
        {
            yield return localisationSettings.LoadAssets();

            string currentTwoLetterISOCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            localisationSettings.SetCurrentLanguage(currentTwoLetterISOCode);
        }

        #endregion

        #region Callbacks

        public void OnSetCurrentLanguage(Language newLanguage)
        {
            localisationSettings.SetCurrentLanguage(newLanguage);
        }

        #endregion
    }
}
