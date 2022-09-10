using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Localisation 
{
	[Serializable]
	public class LanguageValueChangedUnityEvent : ValueChangedUnityEvent<Language> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(LanguageValueChangedEvent), menuName = "Celeste/Events/Localisation/Language Value Changed Event")]
	public class LanguageValueChangedEvent : ParameterisedValueChangedEvent<Language>
	{
	}
}
