using Celeste.Events;
using Celeste.Localisation;
using Celeste.Localisation.Parameters;
using Celeste.Tools.Attributes.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Wallet.UI
{
    [ExecuteInEditMode]
    [AddComponentMenu("Celeste/Wallet/UI/Currency UI Controller")]
    public class CurrencyUIController : MonoBehaviour
    {
        #region Properties and Fields

        [Header("Data")]
        [SerializeField] private Currency currency;
        [SerializeField] private AnimatedCurrencyTransformCache animatedCurrencyTransformCache;

        [Header("UI Elements")]
        [SerializeField] private Image currencyIcon;
        [SerializeField] private TextMeshProUGUI quantityText;

        [Header("Settings")]
        [SerializeField] private bool isCurrencyTarget = true;
        [SerializeField] private bool truncateCurrency = true;
        [SerializeField, ShowIf(nameof(truncateCurrency))] private LanguageValue currentLanguage;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (currency != null)
            {
                currencyIcon.sprite = currency.Icon;
            }
        }

        private void OnEnable()
        {
            if (currency != null)
            {
                currency.AddOnQuantityChangedCallback(OnCurrencyChanged);

                if (isCurrencyTarget)
                {
                    animatedCurrencyTransformCache?.AddCurrencyTarget(currency, GetComponent<RectTransform>());
                }

                if (truncateCurrency && currentLanguage != null)
                {
                    currentLanguage.AddValueChangedCallback(OnCurrentLanguageChanged);
                }

                UpdateUI();
            }
        }

        private void OnDisable()
        {
            if (currency != null)
            {
                if (isCurrencyTarget)
                {
                    animatedCurrencyTransformCache?.RemoveCurrencyTarget(currency);
                }

                if (truncateCurrency && currentLanguage != null)
                {
                    currentLanguage.RemoveValueChangedCallback(OnCurrentLanguageChanged);
                }

                currency.RemoveOnQuantityChangedCallback(OnCurrencyChanged);
            }
        }

        private void Start()
        {
            UpdateUI();
        }

        #endregion

        public void UpdateUI()
        {
            if (currency != null)
            {
                quantityText.text = truncateCurrency && currentLanguage != null
                    ? currentLanguage.Truncate(currency.Quantity)
                    : currency.Quantity.ToString();
            }
        }

        #region Callbacks

        private void OnCurrencyChanged(ValueChangedArgs<int> args)
        {
            UpdateUI();
        }

        private void OnCurrentLanguageChanged(ValueChangedArgs<Language> args)
        {
            UpdateUI();
        }

        #endregion
    }
}