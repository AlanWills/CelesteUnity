using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class StringValueChangedUnityEvent : ValueChangedUnityEvent<string> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(StringValueChangedEvent), menuName = "Celeste/Events/String/String Value Changed Event")]
	public class StringValueChangedEvent : ParameterisedValueChangedEvent<string>
	{
	}
}
