using Celeste.Parameters;
using System.Collections;
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

        #endregion

        #region Unity Methods

        private void Awake()
        {
            currencyIcon.sprite = currency.Icon;
        }

        private void OnEnable()
        {
            currency.AddOnQuantityChangedCallback(OnCurrencyChanged);

            if (isCurrencyTarget)
            {
                animatedCurrencyTransformCache.AddCurrencyTarget(currency, GetComponent<RectTransform>());
            }

            UpdateUI();
        }

        private void OnDisable()
        {
            if (isCurrencyTarget)
            {
                animatedCurrencyTransformCache.RemoveCurrencyTarget(currency);
            }

            currency.RemoveOnQuantityChangedCallback(OnCurrencyChanged);
        }

        private void Start()
        {
            UpdateUI();
        }

        #endregion

        public void UpdateUI()
        {
            quantityText.text = currency.Quantity.ToString();
        }

        #region Callbacks

        private void OnCurrencyChanged(ValueChangedArgs<int> args)
        {
            UpdateUI();
        }

        #endregion
    }
}