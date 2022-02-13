using Celeste.Assets.AssetReferences;
using Celeste.Localisation.Catalogue;
using System;

namespace Celeste.Localisation.AssetReferences
{
    [Serializable]
    public class AssetReferenceLanguageCatalogue : CelesteAssetReference<LanguageCatalogue>
    {
        public AssetReferenceLanguageCatalogue(string guid) : base(guid)
        {
        }
    }
}