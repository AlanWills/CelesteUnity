using Celeste.Rewards.Catalogue;
using Celeste.Rewards.Objects;
using Celeste.Shop.Catalogue;
using UnityEngine;

namespace Celeste.Shop.Purchasing
{
    [CreateAssetMenu(fileName = nameof(ShopPurchaser), order = CelesteMenuItemConstants.SHOP_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SHOP_MENU_ITEM + "Shop Purchaser")]
    public class ShopPurchaser : ScriptableObject
    {
        public void Purchase(ShopItem shopItem)
        {
            var cost = shopItem.Cost;

            if (cost.CanAfford)
            {
                cost.currency.Quantity -= cost.quantity;
                shopItem.Reward.AwardReward(1);
            }
        }
    }
}
