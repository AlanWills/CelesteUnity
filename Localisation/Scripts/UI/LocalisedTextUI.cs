﻿using Celeste.Localisation.Parameters;
using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.Localisation.UI
{
    [ExecuteInEditMode, AddComponentMenu("Celeste/Localisation/Localised Text UI")]
    public class LocalisedTextUI : MonoBehaviour
    {
        #region Properties and Fields

        public LocalisationKey Key { get; private set; }
        public string Text { get; private set; }

        [SerializeField] private bool dynamic = false;
        [SerializeField, HideIf(nameof(dynamic))] private LanguageValue currentLanguage;
        [SerializeField, HideIf(nameof(dynamic))] private LocalisationKey key;
        [SerializeField] private TextMeshProUGUI text;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!dynamic && currentLanguage == null)
            {
                currentLanguage = Settings.LocalisationSettings.instance.currentLanguageValue;
                UnityEditor.EditorUtility.SetDirty(this);
            }
            else if (dynamic && currentLanguage != null)
            {
                currentLanguage = null;
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
            this.TryGet(ref text);
        }

        private void OnEnable()
        {
            Localise();

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
                Localise();
            }
        }
#endif

#endregion

        public void Localise(LocalisationKey key, Language language)
        {
            Key = key;
            Text = language.Localise(key);

            text.text = Text;
        }

        private void Localise()
        {
            if (!dynamic && key != null && currentLanguage != null)
            {
                Localise(key, currentLanguage.Value);
            }
        }

        #region Callbacks

        private void OnCurrentLanguageChanged(Language language)
        {
            Localise();
        }

        #endregion
    }
}