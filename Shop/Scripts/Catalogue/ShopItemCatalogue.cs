using Celeste.Objects;
using UnityEngine;

namespace Celeste.Shop.Catalogue
{
    [CreateAssetMenu(fileName = nameof(ShopItemCatalogue), order = CelesteMenuItemConstants.SHOP_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SHOP_MENU_ITEM + "Shop Item Catalogue")]
    public class ShopItemCatalogue : ListScriptableObject<ShopItem>
    {
        public ShopItem FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}
