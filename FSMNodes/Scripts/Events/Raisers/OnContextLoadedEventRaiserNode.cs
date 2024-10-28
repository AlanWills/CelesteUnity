using Celeste.Events;
using Celeste.Parameters;
using Celeste.Scene.Events;

namespace Celeste.FSM.Nodes.Events
{
    [CreateNodeMenu("Celeste/Events/Raisers/On Context Loaded Event Raiser")]
    public class OnContextLoadedEventRaiserNode : ParameterisedEventRaiserNode<OnContextLoadedArgs, OnContextLoadedEvent>
    {
    }
}
