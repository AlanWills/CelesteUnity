using System;
using UnityEngine;

namespace Celeste.Events 
{
	[Serializable]
	public class Vector2ValueChangedUnityEvent : ValueChangedUnityEvent<Vector2> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(Vector2ValueChangedEvent), menuName = "Celeste/Events/Vector2/Vector2 Value Changed Event")]
	public class Vector2ValueChangedEvent : ParameterisedValueChangedEvent<Vector2>
	{
	}
}
