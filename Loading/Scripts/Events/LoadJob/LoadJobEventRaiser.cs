using Celeste.Events;
using UnityEngine;

namespace Celeste.Loading.Events
{
    [AddComponentMenu("Celeste/Events/Loading/Load Job Event Raiser")]
    public class LoadJobEventRaiser : ParameterisedEventRaiser<LoadJob, LoadJobEvent>
    {
    }
}
