using Celeste.Scene;
using Celeste.Scene.Catalogue;
using CelesteEditor.Objects;

namespace CelesteEditor.Scene
{
    public class SceneSetCataloguePostProcessor : CataloguePostProcessor<SceneSet, SceneSetCatalogue>
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            HandleOnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths, didDomainReload);
        }
    }
}
