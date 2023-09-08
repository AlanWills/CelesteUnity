using System;
using UnityEngine;
using Celeste.Events;
using Celeste.Sound.Settings;

namespace Celeste.Events
{
	[Serializable]
	public class AudioClipSettingsValueChangedUnityEvent : ValueChangedUnityEvent<AudioClipSettings> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(AudioClipSettingsValueChangedEvent), menuName = "Celeste/Events/Audio/Audio Clip Settings Value Changed Event")]
	public class AudioClipSettingsValueChangedEvent : ParameterisedValueChangedEvent<AudioClipSettings>
	{
	}
}
