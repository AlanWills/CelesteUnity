using Celeste.Wallet;
using System;

namespace Celeste.Shop.Purchasing
{
    [Serializable]
    public struct Cost
    {
        public bool CanAfford => currency.Quantity >= quantity;

        public int quantity;
        public Currency currency;
    }
}
