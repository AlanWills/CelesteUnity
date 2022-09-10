using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class TransformValueChangedUnityEvent : ValueChangedUnityEvent<Transform> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(TransformValueChangedEvent), menuName = "Celeste/Events/Transform/Transform Value Changed Event")]
	public class TransformValueChangedEvent : ParameterisedValueChangedEvent<Transform>
	{
	}
}
