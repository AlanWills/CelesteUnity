using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class Vector3ValueChangedUnityEvent : ValueChangedUnityEvent<Vector3> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(Vector3ValueChangedEvent), menuName = "Celeste/Events/Vector/Vector3 Value Changed Event")]
	public class Vector3ValueChangedEvent : ParameterisedValueChangedEvent<Vector3>
	{
	}
}
