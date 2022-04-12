using Celeste.Assets.AssetReferences;
using System;

namespace Celeste.Inventory.AssetReferences
{
    [Serializable]
    public class InventoryItemAssetReference : CelesteAssetReference<InventoryItem>
    {
        public InventoryItemAssetReference(string guid) : base(guid)
        {
        }
    }
}
