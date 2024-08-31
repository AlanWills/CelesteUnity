using Celeste.Objects;
using Celeste.Shop.Objects;
using UnityEngine;

namespace Celeste.Shop.Catalogue
{
    [CreateAssetMenu(fileName = nameof(IAPCatalogue), order = CelesteMenuItemConstants.SHOP_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SHOP_MENU_ITEM + "IAP Catalogue")]
    public class IAPCatalogue : ListScriptableObject<IAP>
    {
        public IAP FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }

        public IAP FindByCode(string code)
        {
            return FindItem(x => string.CompareOrdinal(x.IAPCode, code) == 0);
        }
    }
}
