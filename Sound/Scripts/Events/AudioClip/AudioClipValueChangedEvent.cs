using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class AudioClipValueChangedUnityEvent : ValueChangedUnityEvent<AudioClip> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(AudioClipValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Audio/Audio Clip Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class AudioClipValueChangedEvent : ParameterisedValueChangedEvent<AudioClip>
	{
	}
}
