using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Celeste.Events
{
    [Serializable]
    public class TilemapUnityEvent : UnityEvent<Tilemap> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(TilemapEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Tilemap/Tilemap Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class TilemapEvent : ParameterisedEvent<Tilemap>
    {
    }
}
