using Celeste.Events;
using Celeste.FSM.Nodes.Events;

namespace Celeste.Narrative.Nodes.Events
{
    [CreateNodeMenu("Celeste/Events/Raisers/Set Background Event Raiser")]
    public class SetBackgroundEventRaiserNode : ParameterisedEventRaiserNode<SetBackgroundEventArgs, SetBackgroundEvent>
    {
    }
}