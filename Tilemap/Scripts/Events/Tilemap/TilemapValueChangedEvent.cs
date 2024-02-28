using System;
using UnityEngine;
using Celeste.Events;
using UnityEngine.Tilemaps;

namespace Celeste.Events 
{
	[Serializable]
	public class TilemapValueChangedUnityEvent : ValueChangedUnityEvent<Tilemap> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(TilemapValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Tilemap/Tilemap Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class TilemapValueChangedEvent : ParameterisedValueChangedEvent<Tilemap>
	{
	}
}
