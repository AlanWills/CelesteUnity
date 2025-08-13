using Celeste.Narrative;
using CelesteEditor.Objects;

namespace CelesteEditor.Narrative
{
    public class ChapterCataloguePostProcessor: CataloguePostProcessor<Chapter, ChapterCatalogue>
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            HandleOnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths, didDomainReload);
        }
    }
}