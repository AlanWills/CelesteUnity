using Celeste.Objects;
using System.Collections;
using UnityEngine;

namespace Celeste.Wallet
{
    [CreateAssetMenu(fileName = nameof(CurrencyCatalogue), order = CelesteMenuItemConstants.WALLET_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.WALLET_MENU_ITEM + "Currency Catalogue")]
    public class CurrencyCatalogue : ArrayScriptableObject<Currency>
    {
        public Currency FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}