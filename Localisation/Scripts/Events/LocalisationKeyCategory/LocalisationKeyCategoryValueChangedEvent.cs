using System;
using UnityEngine;
using Celeste.Events;
using Celeste.Localisation;

namespace Celeste.Events 
{
	[Serializable]
	public class LocalisationKeyCategoryValueChangedUnityEvent : ValueChangedUnityEvent<LocalisationKeyCategory> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(LocalisationKeyCategoryValueChangedEvent), menuName = "Celeste/Events/Localisation/Localisation Key Category Value Changed Event")]
	public class LocalisationKeyCategoryValueChangedEvent : ParameterisedValueChangedEvent<LocalisationKeyCategory>
	{
	}
}
