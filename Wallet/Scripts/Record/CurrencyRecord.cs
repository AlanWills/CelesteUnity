using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Wallet.Record
{
    public class CurrencyRecord
    {
        #region Properties and Fields

        public Currency Currency { get; }
        
        public int Quantity
        {
            get { return quantity; }
            set
            {
                if (value != quantity)
                {
                    quantity = value;
                    Currency.OnQuantityChanged.Invoke(quantity);
                }
            }
        }

        [NonSerialized] private int quantity;

        #endregion

        public CurrencyRecord(Currency currency)
        {
            Currency = currency;
            Quantity = currency.StartingQuantity;
        }

        public CurrencyRecord(Currency currency, int quantity)
        {
            Currency = currency;
            Quantity = quantity;
        }
    }
}