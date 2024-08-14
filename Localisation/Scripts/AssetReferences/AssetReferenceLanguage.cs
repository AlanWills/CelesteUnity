#if USE_ADDRESSABLES
using Celeste.Assets.AssetReferences;
using System;

namespace Celeste.Localisation.AssetReferences
{
    [Serializable]
    public class AssetReferenceLanguage : CelesteAssetReference<Language>
    {
        public AssetReferenceLanguage(string guid) : base(guid)
        {
        }
    }
}
#endif