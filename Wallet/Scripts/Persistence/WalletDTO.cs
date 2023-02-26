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
            currencies = new List<CurrencyDTO>(walletRecord.NumCurrencies);

            for (int i = 0, n = walletRecord.NumCurrencies; i < n; ++i)
            {
                Currency currency = walletRecord.GetCurrency(i);

                if (currency.IsPersistent)
                {
                    currencies.Add(new CurrencyDTO(currency.Guid, currency.Quantity));
                }
            }
        }
    }
}