using System.ComponentModel;
using Celeste.FSM.Nodes.Events.Conditions;
using Celeste.Narrative.Events;

namespace Celeste.Narrative.Nodes.Conditions
{
        [DisplayName("Narrative Runtime")]
        public class NarrativeRuntimeEventCondition : ParameterizedEventCondition<NarrativeRuntime, NarrativeRuntimeEvent>
        {
        }
}