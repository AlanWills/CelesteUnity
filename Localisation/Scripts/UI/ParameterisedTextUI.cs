using Celeste.Events;
using Celeste.Localisation.Parameters;
using Celeste.Parameters;
using Celeste.Tools;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Celeste.Localisation.UI
{
    [ExecuteInEditMode, AddComponentMenu("Celeste/Localisation/Parameterised Text UI")]
    public class ParameterisedTextUI : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private LanguageValue currentLanguage;
        [SerializeField] private LocalisationKey key;
        [SerializeField] private TextMeshProUGUI text;

        [NonSerialized] private List<ValueTuple<string, string>> locaTokens = new List<(string, string)>();

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref text);

#if UNITY_EDITOR
            if (currentLanguage == null)
            {
                currentLanguage = Settings.LocalisationEditorSettings.GetOrCreateSettings().currentLanguageValue;
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }

        private void OnEnable()
        {
            Localise();

            if (currentLanguage != null)
            {
                currentLanguage.AddValueChangedCallback(OnCurrentLanguageChanged);
            }
        }

        private void OnDisable()
        {
            if (currentLanguage != null)
            {
                currentLanguage.RemoveValueChangedCallback(OnCurrentLanguageChanged);
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!Application.isPlaying)
            {
                Localise();
            }
        }
#endif

        #endregion

        public void Setup(LocalisationKey localisationKey, params string[] locaTokens)
        {
            key = localisationKey;
            
            Setup(locaTokens);
        }

        public void Setup(params string[] locaTokens)
        {
            for (int i = 0, n = locaTokens != null ? locaTokens.Length : 0; i < n - 1; i += 2)
            {
                this.locaTokens.Add(new ValueTuple<string, string>(locaTokens[i], locaTokens[i+1]));
            }

            Localise();
        }

        private void Localise(LocalisationKey key, Language language)
        {
            string localisedText = language != null ? language.Localise(key) : key.Fallback;

            for (int i = 0, n = locaTokens.Count; i < n; ++i)
            {
                localisedText = localisedText.Replace($"{{{locaTokens[i].Item1}}}", locaTokens[i].Item2);
            }

            text.text = localisedText;
        }

        private void Localise()
        {
            if (key != null && currentLanguage != null)
            {
                Localise(key, currentLanguage.Value);
            }
        }

        #region Callbacks

        private void OnCurrentLanguageChanged(ValueChangedArgs<Language> language)
        {
            Localise();
        }

        #endregion
    }
}