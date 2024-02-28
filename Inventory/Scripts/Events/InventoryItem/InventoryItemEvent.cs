using Celeste.Inventory;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class InventoryItemUnityEvent : UnityEvent<InventoryItem> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(InventoryItemEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Inventory/Inventory Item Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class InventoryItemEvent : ParameterisedEvent<InventoryItem>
    {
    }
}
