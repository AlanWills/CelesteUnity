using Celeste.Events;
using System.ComponentModel;

namespace Celeste.FSM.Nodes.Events.Conditions
{
    [DisplayName("Int")]
    public class IntEventCondition : ParameterizedEventCondition<int, IntEvent>
    {
    }
}
