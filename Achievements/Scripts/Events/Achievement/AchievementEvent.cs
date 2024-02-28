using System;
using Celeste.Achievements.Objects;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;

namespace Celeste.Achievements.Events
{
	[Serializable]
	public class AchievementUnityEvent : UnityEvent<Achievement> { }
	
	[Serializable]
	[CreateAssetMenu(fileName = nameof(AchievementEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Achievements/Achievement", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class AchievementEvent : ParameterisedEvent<Achievement> { }
	
	[Serializable]
	public class GuaranteedAchievementEvent : GuaranteedParameterisedEvent<AchievementEvent, Achievement> { }
}
