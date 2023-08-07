using Celeste.Events;
using System.ComponentModel;

namespace Celeste.FSM.Nodes.Events.Conditions
{
    [DisplayName("Int Value Changed")]
    public class IntValueChangedEventCondition : ParameterizedEventCondition<ValueChangedArgs<int>, IntValueChangedEvent>
    {
    }
}
