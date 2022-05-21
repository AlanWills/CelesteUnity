using Celeste.Events;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Wallet.Events
{
    [Serializable]
    public struct CurrencyChangedArgs
    {
        public Currency currency;
        public int quantity;

        public CurrencyChangedArgs(Currency currency, int quantity)
        {
            this.currency = currency;
            this.quantity = quantity;
        }
    }

    [Serializable]
    public class CurrencyChangedUnityEvent : UnityEvent<CurrencyChangedArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(CurrencyChangedEvent), menuName = "Celeste/Events/Wallet/Currency Changed Event")]
    public class CurrencyChangedEvent : ParameterisedEvent<CurrencyChangedArgs>
    {
    }
}
