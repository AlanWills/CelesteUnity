using Celeste.Assets.AssetReferences;
using Celeste.Localisation.Catalogue;
using System;

namespace Celeste.Localisation.AssetReferences
{
    [Serializable]
    public class AssetReferenceLanguageCatalogue : CelesteAssetReference<LanguageCatalogue>
    {
        public int NumItems => ShouldLoad ? 0 : Asset.NumItems;

        public AssetReferenceLanguageCatalogue(string guid) : base(guid)
        {
        }

        public Language GetItem(int index)
        {
            return ShouldLoad ? null : GetItem(index);
        }
    }
}