using Celeste.Localisation.Parameters;
using Celeste.Parameters;
using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
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

        [SerializeField] private LanguageValue currentLanguage;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private bool dynamic = false;
        [SerializeField, HideIf(nameof(dynamic))] private LocalisationKey key;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (currentLanguage == null)
            {
                currentLanguage = Settings.LocalisationEditorSettings.GetOrCreateSettings().currentLanguageValue;
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

        public void Localise(LocalisationKey key, Language language)
        {
            Key = key;
            Text = language != null ? language.Localise(key) : key.Fallback;

            text.text = Text;
        }

        public void Localise(LocalisationKey key)
        {
            UnityEngine.Debug.Assert(currentLanguage != null, $"No current language set on {nameof(LocalisedTextUI)} component on {name}.");
            Localise(key, currentLanguage.Value);
        }

        private void Localise()
        {
            if (!dynamic && key != null && currentLanguage != null)
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