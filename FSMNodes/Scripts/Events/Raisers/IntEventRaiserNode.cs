using Celeste.Events;
using Celeste.Parameters;

namespace Celeste.FSM.Nodes.Events
{
    [CreateNodeMenu("Celeste/Events/Raisers/IntEvent Raiser")]
    public class IntEventRaiserNode : ParameterisedEventRaiserNode<int, IntValue, IntReference, IntEvent>
    {
    }
}
