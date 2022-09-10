using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class KeyCodeValueChangedUnityEvent : ValueChangedUnityEvent<KeyCode> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(KeyCodeValueChangedEvent), menuName = "Celeste/Events/Input/Key Code Value Changed Event")]
	public class KeyCodeValueChangedEvent : ParameterisedValueChangedEvent<KeyCode>
	{
	}
}
