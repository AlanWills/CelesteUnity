using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class UIntValueChangedUnityEvent : ValueChangedUnityEvent<uint> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(UIntValueChangedEvent), menuName = "Celeste/Events/Numeric/UInt Value Changed Event")]
	public class UIntValueChangedEvent : ParameterisedValueChangedEvent<uint>
	{
	}
}
