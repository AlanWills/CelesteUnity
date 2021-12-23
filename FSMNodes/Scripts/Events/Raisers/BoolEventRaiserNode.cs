using Celeste.Events;
using Celeste.Parameters;
using System;

namespace Celeste.FSM.Nodes.Events
{
    [Serializable]
    [CreateNodeMenu("Celeste/Events/Raisers/BoolEvent Raiser")]
    public class BoolEventRaiserNode : ParameterisedEventRaiserNode<bool, BoolValue, BoolReference, BoolEvent>
    {
    }
}
