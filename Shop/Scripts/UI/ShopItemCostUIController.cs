using Celeste.Events;
using Celeste.Localisation;
using Celeste.Localisation.UI;
using System;
using UnityEngine;

namespace Celeste.Shop.UI
{
    [AddComponentMenu("Celeste/Shop/UI/Shop Item Cost UI Controller")]
    public class ShopItemCostUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private LocalisationKey iapPurchaseLocalisationKey;
        [SerializeField] private LocalisationKey currencyPurchaseLocalisationKey;
        [SerializeField] private ParameterisedTextUI costQuantityText;

        [NonSerialized] private ShopItem shopItem;

        #endregion

        public void Hookup(ShopItem _shopItem)
        {
            shopItem = _shopItem;

            RefreshPrice();
            
            if (shopItem.IsIAPPurchase)
            {
                shopItem.IAP.AddOnLocalisedPriceStringChangedCallback(OnLocalisedPriceStringChanged);
            }
        }

        public void RefreshPrice()
        {
            if (shopItem == null)
            {
                return;
            }

            if (shopItem.IsCurrencyPurchase)
            {
                costQuantityText.Setup(
                    currencyPurchaseLocalisationKey,
                    "GLYPH", shopItem.Cost.currency.GlyphName,
                    "COST", shopItem.Cost.quantity.ToString());
            }
            else
            {
                costQuantityText.Setup(
                    iapPurchaseLocalisationKey,
                    "COST", shopItem.IAP.LocalisedPriceString);
            }
        }

        #region Callbacks

        private void OnLocalisedPriceStringChanged(ValueChangedArgs<string> args)
        {
            RefreshPrice();
        }

        #endregion
    }
}
