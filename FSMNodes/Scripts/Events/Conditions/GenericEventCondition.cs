using Celeste.Events;
using System.ComponentModel;

namespace Celeste.FSM.Nodes.Events.Conditions
{
    [DisplayName("Generic")]
    public class GenericEventCondition : ParameterizedEventCondition<IEventArgs, GenericEvent>
    {
    }
}
