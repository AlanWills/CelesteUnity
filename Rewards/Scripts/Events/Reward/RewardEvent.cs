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
	[CreateAssetMenu(fileName = nameof(RewardEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Rewards/Reward Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class RewardEvent : ParameterisedEvent<Reward> { }
	
	[Serializable]
	public class GuaranteedRewardEvent : GuaranteedParameterisedEvent<RewardEvent, Reward> { }
}
