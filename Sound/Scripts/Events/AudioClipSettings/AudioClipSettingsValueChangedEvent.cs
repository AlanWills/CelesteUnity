using System;
using UnityEngine;
using Celeste.Events;
using Celeste.Sound.Settings;

namespace Celeste.Events
{
	[Serializable]
	public class AudioClipSettingsValueChangedUnityEvent : ValueChangedUnityEvent<AudioClipSettings> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(AudioClipSettingsValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Audio/Audio Clip Settings Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class AudioClipSettingsValueChangedEvent : ParameterisedValueChangedEvent<AudioClipSettings>
	{
	}
}
