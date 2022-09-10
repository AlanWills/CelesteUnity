using System;
using UnityEngine;
using Celeste.Events;
using System.Collections.Generic;

namespace Celeste.Events 
{
	[Serializable]
	public class Vector3IntArrayValueChangedUnityEvent : ValueChangedUnityEvent<List<Vector3Int>> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(Vector3IntArrayValueChangedEvent), menuName = "Celeste/Events/Vector/Vector3 Int Array Value Changed Event")]
	public class Vector3IntArrayValueChangedEvent : ParameterisedValueChangedEvent<List<Vector3Int>>
	{
	}
}
