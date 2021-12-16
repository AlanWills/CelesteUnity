using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.Wallet.UI
{
    [AddComponentMenu("Celeste/Wallet/UI/Currency UI Controller")]
    public class CurrencyUIController : MonoBehaviour
    {
        #region Properties and Fields

        [Header("Data")]
        [SerializeField] private Currency currency;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI quantityText;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            currency.AddOnQuantityChangedCallback(OnCurrencyChanged);
            
            UpdateUI();
        }

        private void OnDisable()
        {
            currency.RemoveOnQuantityChangedCallback(OnCurrencyChanged);
        }

        #endregion

        public void UpdateUI()
        {
            quantityText.text = currency.Quantity.ToString();
        }

        #region Callbacks

        private void OnCurrencyChanged(int newQuantity)
        {
            UpdateUI();
        }

        #endregion
    }
}