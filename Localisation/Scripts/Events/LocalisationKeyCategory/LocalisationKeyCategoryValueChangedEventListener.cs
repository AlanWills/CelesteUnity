using UnityEngine;
using Celeste.Events;
using Celeste.Localisation;

namespace Celeste.Events
{
	public class LocalisationKeyCategoryValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<LocalisationKeyCategory>, LocalisationKeyCategoryValueChangedEvent, LocalisationKeyCategoryValueChangedUnityEvent>
	{
	}
}
