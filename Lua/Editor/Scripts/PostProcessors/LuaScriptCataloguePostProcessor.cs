using Celeste.Lua.Catalogues;
using CelesteEditor.Objects;
using Lua.Unity;

namespace CelesteEditor.Lua.PostProcessors
{
    public class LuaScriptCataloguePostProcessor : CataloguePostProcessor<LuaScript, LuaScriptCatalogue>
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            HandleOnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths, didDomainReload);
        }
    }
}