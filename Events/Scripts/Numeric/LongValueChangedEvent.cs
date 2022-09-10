using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class LongValueChangedUnityEvent : ValueChangedUnityEvent<long> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(LongValueChangedEvent), menuName = "Celeste/Events/Numeric/Long Value Changed Event")]
	public class LongValueChangedEvent : ParameterisedValueChangedEvent<long>
	{
	}
}
