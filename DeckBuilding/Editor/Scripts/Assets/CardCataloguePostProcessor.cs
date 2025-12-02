using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Catalogue;
using CelesteEditor.Objects;

namespace CelesteEditor.DeckBuilding.Assets
{
    public class CardCataloguePostProcessor : CataloguePostProcessor<Card, CardCatalogue>
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            HandleOnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths, didDomainReload);
        }
    }
}