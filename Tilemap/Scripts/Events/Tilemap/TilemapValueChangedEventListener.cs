using UnityEngine;
using Celeste.Events;
using UnityEngine.Tilemaps;

namespace Celeste.Events
{
	public class TilemapValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<Tilemap>, TilemapValueChangedEvent, TilemapValueChangedUnityEvent>
	{
	}
}
