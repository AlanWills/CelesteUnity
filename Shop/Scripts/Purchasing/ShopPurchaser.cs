using Celeste.Rewards.Catalogue;
using Celeste.Shop.Catalogue;
using UnityEngine;

namespace Celeste.Shop.Purchasing
{
    [CreateAssetMenu(fileName = nameof(ShopPurchaser), menuName = "Celeste/Shop/Shop Purchaser")]
    public class ShopPurchaser : ScriptableObject
    {
        public void PurchaseWithCurrency(ShopItem shopItem)
        {
            var cost = shopItem.Cost;

            if (cost.CanAfford)
            {
                cost.currency.Quantity -= cost.quantity;

                foreach (Reward reward in shopItem.Rewards)
                {
                    reward.AwardReward();
                }
            }
        }
    }
}
