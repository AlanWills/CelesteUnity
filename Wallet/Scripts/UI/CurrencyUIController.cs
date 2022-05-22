using Celeste.Parameters;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Wallet.UI
{
    [AddComponentMenu("Celeste/Wallet/UI/Currency UI Controller")]
    public class CurrencyUIController : MonoBehaviour
    {
        #region Properties and Fields

        [Header("Data")]
        [SerializeField] private Currency currency;

        [Header("UI Elements")]
        [SerializeField] private Image currencyIcon;
        [SerializeField] private TextMeshProUGUI quantityText;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            currencyIcon.sprite = currency.Icon;
        }

        private void OnEnable()
        {
            currency.AddOnQuantityChangedCallback(OnCurrencyChanged);

            UpdateUI();
        }

        private void OnDisable()
        {
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