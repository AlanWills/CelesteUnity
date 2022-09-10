using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class GameObjectValueChangedUnityEvent : ValueChangedUnityEvent<GameObject> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(GameObjectValueChangedEvent), menuName = "Celeste/Events/Game Object/Game Object Value Changed Event")]
	public class GameObjectValueChangedEvent : ParameterisedValueChangedEvent<GameObject>
	{
	}
}
