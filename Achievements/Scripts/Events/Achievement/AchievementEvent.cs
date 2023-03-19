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
	[CreateAssetMenu(fileName = nameof(AchievementEvent), menuName = "Celeste/Events/Achievements/Achievement")]
	public class AchievementEvent : ParameterisedEvent<Achievement> { }
	
	[Serializable]
	public class GuaranteedAchievementEvent : GuaranteedParameterisedEvent<AchievementEvent, Achievement> { }
}
