using Celeste.Events;
using UnityEngine;

namespace Celeste.Wallet.Events
{
    [AddComponentMenu("Celeste/Events/Wallet/Currency Changed Event Listener")]
    public class CurrencyChangedEventListener : ParameterisedEventListener<CurrencyChangedArgs, CurrencyChangedEvent, CurrencyChangedUnityEvent>
    {
    }
}
