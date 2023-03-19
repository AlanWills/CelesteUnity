using Celeste.Localisation.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Shop.UI
{
    [AddComponentMenu("Celeste/Shop/UI/Shop Item UI Controller")]
    public class ShopItemUIController : MonoBehaviour, IShopItemUIController
    {
        #region Properties and Fields

        [SerializeField] private LocalisedTextUI shopItemTitle;
        [SerializeField] private Image shopItemIcon;
        [SerializeField] private ShopItemCostUIController shopItemCost;
        [SerializeField] private ShopItemPurchaseUIController shopItemPurchase;

        #endregion
        
        public void Hookup(ShopItemLayout shopItemLayout)
        {
            shopItemTitle.Localise(shopItemLayout.shopItem.Title);
            shopItemIcon.sprite = shopItemLayout.shopItem.Icon;
            shopItemCost.Hookup(shopItemLayout.shopItem);
            shopItemPurchase.Hookup(shopItemLayout.shopItem);
        }
    }
}