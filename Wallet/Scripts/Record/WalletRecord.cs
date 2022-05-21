using Celeste.DataStructures;
using Celeste.Wallet.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Celeste.Wallet.Currency;

namespace Celeste.Wallet
{
    [CreateAssetMenu(fileName = nameof(WalletRecord), menuName = "Celeste/Wallet/Wallet Record")]
    public class WalletRecord : ScriptableObject
    {
        #region Properties and Fields

        public int NumCurrencies
        {
            get { return currencies.Count; }
        }

        [Header("Events")]
        [SerializeField] private CurrencyChangedEvent currencyChanged;
        [SerializeField] private Celeste.Events.Event save;

        [NonSerialized] private List<Currency> currencies = new List<Currency>();

        #endregion

        public void Initialize(CurrencyCatalogue currencyCatalogue)
        {
            for (int i = 0, n = currencyCatalogue.NumItems; i < n; ++i)
            {
                Currency currency = currencyCatalogue.GetItem(i);
                currency.AddOnQuantityChangedCallback((quantity) =>
                {
                    currencyChanged.Invoke(new CurrencyChangedArgs(currency, quantity));
                    save.Invoke();
                });
                currencies.Add(currency);
            }
        }

        public void Shutdown()
        {
            for (int i = 0, n = currencies.Count; i < n; ++i)
            {
                currencies[i].RemoveAllQuantityChangedCallbacks();
            }

            currencies.Clear();
        }

        public void CreateStartingWallet()
        {
            foreach (Currency currency in currencies)
            {
                currency.Quantity = currency.StartingQuantity;
            }
        }

        public Currency GetCurrency(int recordIndex)
        {
            return currencies.Get(recordIndex);
        }
    }
}