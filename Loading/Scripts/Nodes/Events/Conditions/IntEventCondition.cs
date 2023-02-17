using Celeste.Events;
using Celeste.FSM.Nodes.Events.Conditions;
using Celeste.Scene.Events;
using System.ComponentModel;

namespace Celeste.Loading.Nodes
{
    [DisplayName("On Context Loaded")]
    public class OnContextLoadedEventCondition : ParameterizedEventCondition<OnContextLoadedArgs, OnContextLoadedEvent>
    {
    }
}
