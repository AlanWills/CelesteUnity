using System;
using UnityEngine;

namespace Celeste.Events 
{
	[Serializable]
	public class Vector3IntValueChangedUnityEvent : ValueChangedUnityEvent<Vector3Int> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(Vector3IntValueChangedEvent), menuName = "Celeste/Events/Vector3Int/Vector3 Int Value Changed Event")]
	public class Vector3IntValueChangedEvent : ParameterisedValueChangedEvent<Vector3Int>
	{
	}
}
