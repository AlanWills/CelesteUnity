using System;
using UnityEngine;
using Celeste.Events;
using UnityEngine.InputSystem;

namespace Celeste.Events 
{
	[Serializable]
	public class KeyValueChangedUnityEvent : ValueChangedUnityEvent<Key> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(KeyValueChangedEvent), menuName = "Celeste/Events/Input/Key Value Changed Event")]
	public class KeyValueChangedEvent : ParameterisedValueChangedEvent<Key>
	{
	}
}
