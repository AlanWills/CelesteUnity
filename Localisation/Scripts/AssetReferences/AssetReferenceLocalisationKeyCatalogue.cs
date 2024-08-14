#if USE_ADDRESSABLES
using Celeste.Assets.AssetReferences;
using Celeste.Localisation.Catalogue;
using System;

namespace Celeste.Localisation.AssetReferences
{
    [Serializable]
    public class AssetReferenceLocalisationKeyCategoryCatalogue : CelesteAssetReference<LocalisationKeyCategoryCatalogue>
    {
        public AssetReferenceLocalisationKeyCategoryCatalogue(string guid) : base(guid)
        {
        }
    }
}
#endif