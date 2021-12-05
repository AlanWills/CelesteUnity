using Celeste.DataStructures;
using Celeste.Wallet.Record;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Wallet
{
    [CreateAssetMenu(fileName = nameof(WalletRecord), menuName = "Celeste/Wallet/Wallet Record")]
    public class WalletRecord : ScriptableObject
    {
        #region Properties and Fields

        public int NumCurrencyRecords
        {
            get { return currencyRecords.Count; }
        }

        [Header("Events")]
        [SerializeField] private Events.Event save;

        [NonSerialized] private List<CurrencyRecord> currencyRecords = new List<CurrencyRecord>();

        #endregion

        public void CreateStartingWallet(CurrencyCatalogue currencyCatalogue)
        {
            for (int i = 0, n = currencyCatalogue.NumItems; i < n; ++i)
            {
                currencyRecords.Add(new CurrencyRecord(currencyCatalogue.GetItem(i)));
            }
        }

        public void SetCurrency(Currency currency, int quantity)
        {
            CurrencyRecord currencyRecord = FindOrAddCurrencyRecord(currency);
            currencyRecord.Quantity = quantity;
            save.Invoke();
        }

        public void AddCurrency(Currency currency, int quantity)
        {
            CurrencyRecord currencyRecord = FindOrAddCurrencyRecord(currency);
            currencyRecord.Quantity += quantity;
            save.Invoke();
        }

        public void RemoveCurrency(Currency currency, int quantity)
        {
            CurrencyRecord currencyRecord = FindOrAddCurrencyRecord(currency);
            currencyRecord.Quantity -= quantity;
            save.Invoke();
        }

        public Currency GetCurrency(int recordIndex)
        {
            CurrencyRecord currencyRecord = currencyRecords.Get(recordIndex);
            return currencyRecord != null ? currencyRecord.Currency : null;
        }

        public int GetQuantity(int recordIndex)
        {
            CurrencyRecord currencyRecord = currencyRecords.Get(recordIndex);
            return currencyRecord != null ? currencyRecord.Quantity : 0;
        }

        public int GetQuantity(Currency currency)
        {
            return FindOrAddCurrencyRecord(currency).Quantity;
        }

        private CurrencyRecord FindOrAddCurrencyRecord(Currency currency)
        {
            CurrencyRecord currencyRecord = currencyRecords.Find(x => x.Currency == currency);
            if (currencyRecord == null)
            {
                currencyRecord = new CurrencyRecord(currency, 0);
                currencyRecords.Add(currencyRecord);
                save.Invoke();
            }

            return currencyRecord;
        }
    }
}