using System;
using System.Collections.Generic;

namespace Celeste.Wallet.Persistence
{
    [Serializable]
    public class WalletDTO
    {
        public List<CurrencyDTO> currencies;

        public WalletDTO(WalletRecord walletRecord)
        {
            currencies = new List<CurrencyDTO>(walletRecord.NumCurrencyRecords);

            for (int i = 0, n = walletRecord.NumCurrencyRecords; i < n; ++i)
            {
                currencies.Add(new CurrencyDTO(walletRecord.GetCurrency(i).Guid, walletRecord.GetQuantity(i)));
            }
        }
    }
}