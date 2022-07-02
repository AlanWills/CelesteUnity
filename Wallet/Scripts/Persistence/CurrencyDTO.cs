using System;

namespace Celeste.Wallet.Persistence
{
    [Serializable]
    public class CurrencyDTO
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