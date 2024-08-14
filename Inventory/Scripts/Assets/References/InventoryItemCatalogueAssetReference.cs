#if USE_ADDRESSABLES
using Celeste.Assets.AssetReferences;
using System;

namespace Celeste.Inventory.AssetReferences
{
    [Serializable]
    public class InventoryItemCatalogueAssetReference : CelesteAssetReference<InventoryItemCatalogue>
    {
        public InventoryItemCatalogueAssetReference(string guid) : base(guid)
        {
        }
    }
}
#endif