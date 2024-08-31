using System;
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

        public async void PurchaseShopItem()
        {
            PurchaseResult purchaseResult = await shopPurchaser.Purchase(shopItem);
            Debug.Assert(purchaseResult.success, $"Purchase failed with reason: {purchaseResult.reason} and message: {purchaseResult.errorMessage}.");
        }
    }
}