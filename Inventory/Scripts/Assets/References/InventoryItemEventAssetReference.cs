using Celeste.Assets.AssetReferences;
using Celeste.Events;
using System;

namespace Celeste.Inventory.AssetReferences
{
    [Serializable]
    public class InventoryItemEventAssetReference : CelesteAssetReference<InventoryItemEvent>
    {
        public InventoryItemEventAssetReference(string guid) : base(guid)
        {
        }
    }
}
