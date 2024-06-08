using Celeste.Log;
using CelesteEditor.Objects;

namespace CelesteEditor.Log
{
    public class SectionLogSettingsCataloguePostProcessor : CataloguePostProcessor<SectionLogSettings, SectionLogSettingsCatalogue>
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            HandleOnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths, didDomainReload);
        }
    }
}
