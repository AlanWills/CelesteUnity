using Celeste.Objects;
using UnityEngine;

namespace Celeste.Shop.Catalogue
{
    [CreateAssetMenu(fileName = nameof(ShopItemCatalogue), menuName = "Celeste/Shop/Shop Item Catalogue")]
    public class ShopItemCatalogue : ListScriptableObject<ShopItem>
    {
        public ShopItem FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}
