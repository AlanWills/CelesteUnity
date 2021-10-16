using Celeste.Events;
using UnityEngine;

namespace Celeste.Scene.Events
{
    [AddComponentMenu("Celeste/Events/Loading/On Context Loaded Event Listener")]
    public class OnContextLoadedEventListener : ParameterisedEventListener<OnContextLoadedArgs, OnContextLoadedEvent, OnContextLoadedUnityEvent>
    {
    }
}
