using Celeste.Objects;
using System.Collections;
using UnityEngine;

namespace Celeste.Wallet
{
    [CreateAssetMenu(fileName = nameof(CurrencyCatalogue), menuName = "Celeste/Wallet/Currency Catalogue")]
    public class CurrencyCatalogue : ArrayScriptableObject<Currency>
    {
        public Currency FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}