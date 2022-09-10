using UnityEngine;
using Celeste.Events;
using UnityEngine.Tilemaps;

namespace Celeste.Events
{
	public class TilemapValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<Tilemap>, TilemapValueChangedEvent>
	{
	}
}
