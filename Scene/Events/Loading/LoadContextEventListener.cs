using Celeste.Events;
using UnityEngine;

namespace Celeste.Scene.Events
{
    [AddComponentMenu("Celeste/Events/Loading/Load Context Event Listener")]
    public class LoadContextEventListener : ParameterisedEventListener<LoadContextArgs, LoadContextEvent, LoadContextUnityEvent>
    {
    }
}
