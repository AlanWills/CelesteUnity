#if USE_ADDRESSABLES
using Celeste.Assets.AssetReferences;
using Celeste.Narrative.Backgrounds;
using System;

namespace Celeste.Narrative.Assets.References
{
    [Serializable]
    public class BackgroundCatalogueAssetReference : CelesteAssetReference<BackgroundCatalogue>
    {
        public BackgroundCatalogueAssetReference(string guid) : base(guid)
        {
        }
    }
}
#endif