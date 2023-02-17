using UnityEngine;
using Celeste.Events;
using Celeste.Rewards.Catalogue;

namespace Celeste.Rewards.Events
{
	public class RewardEventListener : ParameterisedEventListener<Reward, RewardEvent, RewardUnityEvent> { }
}
