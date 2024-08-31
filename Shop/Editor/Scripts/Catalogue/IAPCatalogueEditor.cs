using Celeste.Shop.Catalogue;
using Celeste.Shop.Objects;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Shop.Catalogue
{
    [CustomEditor(typeof(IAPCatalogue))]
    public class IAPCatalogueEditor : IIndexableItemsEditor<IAP>
    {
    }
}
