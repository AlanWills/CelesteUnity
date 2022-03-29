using Celeste.Assets.AssetReferences;
using System;

namespace Celeste.Narrative.Assets
{
    [Serializable]
    public class StoryCatalogueAssetReference : CelesteAssetReference<StoryCatalogue>
    {
        public StoryCatalogueAssetReference(string guid) : base(guid)
        {
        }
    }
}
