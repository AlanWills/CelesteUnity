using Celeste.Narrative.Backgrounds;
using CelesteEditor.Objects;

namespace CelesteEditor.Narrative
{
    public class BackgroundCataloguePostProcessor: CataloguePostProcessor<Background, BackgroundCatalogue>
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            HandleOnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths, didDomainReload);
        }
    }
}