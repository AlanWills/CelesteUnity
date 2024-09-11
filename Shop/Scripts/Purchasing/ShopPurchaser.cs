using Celeste.Events;
using Celeste.Shop.Catalogue;
using Celeste.Shop.Purchasing.Impls;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Shop.Purchasing
{
    [CreateAssetMenu(fileName = nameof(ShopPurchaser), order = CelesteMenuItemConstants.SHOP_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SHOP_MENU_ITEM + "Shop Purchaser")]
    public class ShopPurchaser : ScriptableObject
    {
        #region Properties and Fields

        [NonSerialized] private IAPCatalogue iapCatalogue;
        [NonSerialized] private IIAPPurchaser iapPurchaser;

        #endregion

        public void Initialize(IAPCatalogue _iapCatalogue)
        {
            iapCatalogue = _iapCatalogue;

#if UNITY_EDITOR
            iapPurchaser = new EditorIAPPurchaser(_iapCatalogue);
#elif USE_UNITY_PURCHASING
            iapPurchaser = new UnityIAPPurchaser(_iapCatalogue);
#else
            iapPurchaser = new DisabledPurchaser();
#endif
            UnityEngine.Debug.Log($"Created iap purchaser of type: {iapPurchaser.GetType().Name}.");
        }

        public async Task<PurchaseResult> Purchase(ShopItem shopItem)
        {
            if (shopItem.IsCurrencyPurchase)
            {
                var cost = shopItem.Cost;

                if (cost.CanAfford)
                {
                    cost.currency.Quantity -= cost.quantity;
                    shopItem.Reward.AwardReward(1);

                    return new PurchaseResult
                    {
                        success = true
                    };
                }
                else
                {
                    return new PurchaseResult
                    {
                        success = false,
                        reason = PurchaseFailedReason.NotEnoughCurrency,
                        errorMessage = $"Failed to purchase {shopItem.name}.  Needed {cost.quantity} of currency {cost.currency.name}, but had {cost.currency.Quantity}."
                    };
                }
            }
            else
            {
                return await iapPurchaser.PurchaseIAP(shopItem.IAP);
            }
        }
    }
}
