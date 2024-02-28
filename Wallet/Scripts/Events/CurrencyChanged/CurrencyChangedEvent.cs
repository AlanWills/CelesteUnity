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
        public int oldQuantity;
        public int newQuantity;
        public int delta;

        public CurrencyChangedArgs(Currency currency, int oldQuantity, int newQuantity)
        {
            this.currency = currency;
            this.oldQuantity = oldQuantity;
            this.newQuantity = newQuantity;

            delta = newQuantity - oldQuantity;
        }
    }

    [Serializable]
    public class CurrencyChangedUnityEvent : UnityEvent<CurrencyChangedArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(CurrencyChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Wallet/Currency Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class CurrencyChangedEvent : ParameterisedEvent<CurrencyChangedArgs>
    {
    }
}
