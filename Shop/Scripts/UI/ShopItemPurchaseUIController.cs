using System;
using Celeste.Shop.Catalogue;
using Celeste.Shop.Purchasing;
using UnityEngine;

namespace Celeste.Shop.UI
{
    [AddComponentMenu("Celeste/Shop/UI/Shop Item Purchase UI Controller")]
    public class ShopItemPurchaseUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private ShopPurchaser shopPurchaser;

        [NonSerialized] private ShopItem shopItem;
        
        #endregion

        public void Hookup(ShopItem shopItem)
        {
            this.shopItem = shopItem;
        }

        public void PurchaseShopItem()
        {
            shopPurchaser.Purchase(shopItem);
        }
    }
}