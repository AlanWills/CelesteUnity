using Celeste.Shop.Catalogue;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Shop.Catalogue
{
    [CustomEditor(typeof(ShopItemCatalogue))]
    public class ShopItemCatalogueEditor : IIndexableItemsEditor<ShopItem>
    {
    }
}
