using UnityEngine;
using UnityEngine.Tilemaps;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Tilemap/Tilemap Event Listener")]
    public class TilemapEventListener : ParameterisedEventListener<Tilemap, TilemapEvent, TilemapUnityEvent>
    {
    }
}
