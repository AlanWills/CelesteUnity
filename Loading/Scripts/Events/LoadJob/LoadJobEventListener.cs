using Celeste.Events;
using UnityEngine;

namespace Celeste.Loading.Events
{
    [AddComponentMenu("Celeste/Events/Loading/Load Job Event Listener")]
    public class LoadJobEventListener : ParameterisedEventListener<LoadJob, LoadJobEvent, LoadJobUnityEvent>
    {
    }
}
