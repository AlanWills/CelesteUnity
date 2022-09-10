using UnityEngine;
using Celeste.Events;
using Celeste.Localisation;

namespace Celeste.Events
{
	public class LocalisationKeyCategoryValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<LocalisationKeyCategory>, LocalisationKeyCategoryValueChangedEvent>
	{
	}
}
