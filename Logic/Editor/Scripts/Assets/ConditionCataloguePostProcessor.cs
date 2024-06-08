using Celeste.Logic;
using Celeste.Logic.Catalogue;
using CelesteEditor.Objects;

namespace CelesteEditor.Logic.Assets
{
    public class ConditionCataloguePostProcessor : CataloguePostProcessor<Condition, ConditionCatalogue>
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            HandleOnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths, didDomainReload);
        }
    }
}
