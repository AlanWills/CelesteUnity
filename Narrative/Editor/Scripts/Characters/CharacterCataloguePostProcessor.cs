using Celeste.Narrative.Characters;
using CelesteEditor.Objects;

namespace CelesteEditor.Narrative
{
    public class CharacterCataloguePostProcessor: CataloguePostProcessor<Character, CharacterCatalogue>
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            HandleOnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths, didDomainReload);
        }
    }
}