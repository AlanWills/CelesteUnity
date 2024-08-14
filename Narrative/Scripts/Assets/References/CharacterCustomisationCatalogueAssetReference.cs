#if USE_ADDRESSABLES
using Celeste.Assets.AssetReferences;
using Celeste.Narrative.Characters;
using System;

namespace Celeste.Narrative.Assets.References
{
    [Serializable]
    public class CharacterCustomisationCatalogueAssetReference : CelesteAssetReference<CharacterCustomisationCatalogue>
    {
        public CharacterCustomisationCatalogueAssetReference(string guid) : base(guid)
        {
        }
    }
}
#endif