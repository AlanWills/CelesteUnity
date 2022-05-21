using Celeste.Events;
using UnityEngine;

namespace Celeste.Wallet.Events
{
    [AddComponentMenu("Celeste/Events/Wallet/Currency Changed Event Raiser")]
    public class CurrencyChangedEventRaiser : ParameterisedEventRaiser<CurrencyChangedArgs, CurrencyChangedEvent>
    {
    }
}
