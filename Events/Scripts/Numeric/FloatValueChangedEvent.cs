using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class FloatValueChangedUnityEvent : ValueChangedUnityEvent<float> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(FloatValueChangedEvent), menuName = "Celeste/Events/Numeric/Float Value Changed Event")]
	public class FloatValueChangedEvent : ParameterisedValueChangedEvent<float>
	{
	}
}
