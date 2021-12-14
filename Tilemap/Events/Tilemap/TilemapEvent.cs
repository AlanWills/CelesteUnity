using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Celeste.Events
{
    [Serializable]
    public class TilemapUnityEvent : UnityEvent<Tilemap> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(TilemapEvent), menuName = "Celeste/Events/Tilemap/Tilemap Event")]
    public class TilemapEvent : ParameterisedEvent<Tilemap>
    {
    }
}
