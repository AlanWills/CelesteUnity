using UnityEngine;
using Celeste.Events;
using Celeste.Sound.Settings;

namespace Celeste.Events
{
	public class AudioClipSettingsValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<AudioClipSettings>, AudioClipSettingsValueChangedEvent>
	{
	}
}
