using Celeste.UI;
using CelesteEditor.Objects;

namespace CelesteEditor.UI.PostProcessors
{
    public class CanvasLayersPostProcessor : CataloguePostProcessor<CanvasLayer, CanvasLayers>
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            HandleOnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths, didDomainReload);
        }
    }
}