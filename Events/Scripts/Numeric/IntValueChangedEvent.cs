using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class IntValueChangedUnityEvent : ValueChangedUnityEvent<int> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(IntValueChangedEvent), menuName = "Celeste/Events/Numeric/Int Value Changed Event")]
	public class IntValueChangedEvent : ParameterisedValueChangedEvent<int>
	{
	}
}
