using Celeste.Wallet.Record;
using System;

namespace Celeste.Wallet.Persistence
{
    [Serializable]
    public struct CurrencyDTO
    {
        public int currencyGuid;
        public int quantity;

        public CurrencyDTO(int currencyGuid, int quantity)
        {
            this.currencyGuid = currencyGuid;
            this.quantity = quantity;
        }
    }
}