using Celeste.Localisation.Parameters;
using Celeste.Tools;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.Localisation.UI
{
    [ExecuteInEditMode, AddComponentMenu("Celeste/Localisation/UI Localised Text")]
    public class UILocalisedText : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private LanguageValue currentLanguage;
        [SerializeField] private LocalisationKey key;
        [SerializeField] private TextMeshProUGUI text;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref text);
        }

        private void OnEnable()
        {
            UpdateText();

            if (currentLanguage != null)
            {
                currentLanguage.AddOnValueChangedCallback(OnCurrentLanguageChanged);
            }
        }

        private void OnDisable()
        {
            if (currentLanguage != null)
            {
                currentLanguage.RemoveOnValueChangedCallback(OnCurrentLanguageChanged);
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!Application.isPlaying)
            {
                UpdateText();
            }
        }
#endif

#endregion

        private void UpdateText()
        {
            if (currentLanguage != null && key != null)
            {
                text.text = currentLanguage.Value.Localise(key);
            }
        }

        #region Callbacks

        private void OnCurrentLanguageChanged(Language language)
        {
            UpdateText();
        }

        #endregion
    }
}