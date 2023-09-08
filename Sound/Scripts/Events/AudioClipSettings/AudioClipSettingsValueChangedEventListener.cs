using UnityEngine;
using Celeste.Events;
using Celeste.Sound.Settings;

namespace Celeste.Events
{
	public class AudioClipSettingsValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<AudioClipSettings>, AudioClipSettingsValueChangedEvent, AudioClipSettingsValueChangedUnityEvent>
	{
	}
}
