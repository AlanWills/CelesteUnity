using Celeste.FSM.Nodes.Events;
using Celeste.Rewards.Catalogue;
using Celeste.Rewards.Events;

namespace Celeste.Rewards.Nodes.Events
{
    [CreateNodeMenu("Celeste/Events/Raisers/Reward Event Raiser")]
    public class RewardEventRaiserNode : ParameterisedEventRaiserNode<Reward, RewardEvent>
    {
    }
}
