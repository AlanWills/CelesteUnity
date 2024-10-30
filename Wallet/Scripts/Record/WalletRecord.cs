using Celeste.DataStructures;
using Celeste.Wallet.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Wallet
{
    [CreateAssetMenu(fileName = nameof(WalletRecord), order = CelesteMenuItemConstants.WALLET_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.WALLET_MENU_ITEM + "Wallet Record")]
    public class WalletRecord : ScriptableObject
    {
        #region Properties and Fields

        public int NumCurrencies => currencies.Count;

        [Header("Events")]
        [SerializeField] private CurrencyChangedEvent currencyChanged;
        [SerializeField] private Celeste.Events.Event save;

        [NonSerialized] private List<Currency> currencies = new List<Currency>();

        #endregion

        public void Initialize(CurrencyCatalogue currencyCatalogue)
        {
            Dictionary<int, int> startingCurrencyLookup = new Dictionary<int, int>();

            foreach (Currency currency in currencyCatalogue.Items)
            {
                startingCurrencyLookup[currency.Guid] = currency.StartingQuantity;
            }

            Initialize(currencyCatalogue, startingCurrencyLookup);
        }

        public void Initialize(CurrencyCatalogue currencyCatalogue, IReadOnlyDictionary<int, int> currencyQuantityLookup)
        {
            Shutdown();

            for (int i = 0, n = currencyCatalogue.NumItems; i < n; ++i)
            {
                Currency currency = currencyCatalogue.GetItem(i);

                if (currency.IsPersistent && currencyQuantityLookup.TryGetValue(currency.Guid, out int currencyQuantity))
                {
                    currency.Quantity = currencyQuantity;
                }
                
                currency.AddOnQuantityChangedCallback((args) =>
                {
                    currencyChanged.Invoke(new CurrencyChangedArgs(currency, args.oldValue, args.newValue));

                    if (currency.IsPersistent)
                    {
                        // Only save if the currency should be persistent, otherwise it is just a runtime value
                        save.Invoke();
                    }
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

        public Currency GetCurrency(int recordIndex)
        {
            return currencies.Get(recordIndex);
        }
    }
}