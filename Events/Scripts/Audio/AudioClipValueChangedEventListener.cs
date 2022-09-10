using UnityEngine;
using Celeste.Events;

namespace Celeste.Events
{
	public class AudioClipValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<AudioClip>, AudioClipValueChangedEvent, AudioClipValueChangedUnityEvent>
	{
	}
}
