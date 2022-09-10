using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class AudioClipValueChangedUnityEvent : ValueChangedUnityEvent<AudioClip> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(AudioClipValueChangedEvent), menuName = "Celeste/Events/Audio/Audio Clip Value Changed Event")]
	public class AudioClipValueChangedEvent : ParameterisedValueChangedEvent<AudioClip>
	{
	}
}
