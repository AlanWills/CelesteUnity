using Celeste.Events;
using UnityEngine;

namespace Celeste.Scene.Events
{
    [AddComponentMenu("Celeste/Events/Loading/Load Context Event Raiser")]
    public class LoadContextEventRaiser : ParameterisedEventRaiser<LoadContextArgs, LoadContextEvent>
    {
    }
}
