using System;
using UnityEngine;
using Celeste.Events;
using UnityEngine.Tilemaps;

namespace Celeste.Events 
{
	[Serializable]
	public class TilemapValueChangedUnityEvent : ValueChangedUnityEvent<Tilemap> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(TilemapValueChangedEvent), menuName = "Celeste/Events/Tilemap/Tilemap Value Changed Event")]
	public class TilemapValueChangedEvent : ParameterisedValueChangedEvent<Tilemap>
	{
	}
}
