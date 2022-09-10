using System;
using UnityEngine;
using Celeste.Events;

namespace Celeste.Events 
{
	[Serializable]
	public class SpriteValueChangedUnityEvent : ValueChangedUnityEvent<Sprite> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(SpriteValueChangedEvent), menuName = "Celeste/Events/Rendering/Sprite Value Changed Event")]
	public class SpriteValueChangedEvent : ParameterisedValueChangedEvent<Sprite>
	{
	}
}
