using Celeste.Localisation;
using Celeste.Localisation.UI;
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

        #endregion

        public void Hookup(ShopItem shopItem)
        {
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
    }
}
