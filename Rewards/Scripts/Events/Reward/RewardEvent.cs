using System;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;
using Celeste.Rewards.Objects;

namespace Celeste.Rewards.Events
{
	[Serializable]
	public class RewardUnityEvent : UnityEvent<Reward> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(RewardEvent), menuName = "Celeste/Events/Rewards/Reward Event")]
	public class RewardEvent : ParameterisedEvent<Reward> { }
	
	[Serializable]
	public class GuaranteedRewardEvent : GuaranteedParameterisedEvent<RewardEvent, Reward> { }
}
