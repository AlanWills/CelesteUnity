using Celeste.Inventory;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class InventoryItemUnityEvent : UnityEvent<InventoryItem> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(InventoryItemEvent), menuName = "Celeste/Events/Inventory/Inventory Item Event")]
    public class InventoryItemEvent : ParameterisedEvent<InventoryItem>
    {
    }
}
