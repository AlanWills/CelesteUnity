using Celeste.Events;
using UnityEngine;

namespace Celeste.Scene.Events
{
    [AddComponentMenu("Celeste/Events/Loading/On Context Loaded Event Raiser")]
    public class OnContextLoadedEventRaiser : ParameterisedEventRaiser<OnContextLoadedArgs, OnContextLoadedEvent>
    {
    }
}
