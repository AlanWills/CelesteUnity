using Celeste.Events;
using Celeste.FSM.Nodes.Events;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Parameters;

namespace Celeste.Narrative.Nodes.Events
{
    [CreateNodeMenu("Celeste/Events/Raisers/BackgroundEvent Raiser")]
    public class BackgroundEventRaiserNode : ParameterisedEventRaiserNode<Background, BackgroundValue, BackgroundReference, BackgroundEvent>
    {
    }
}