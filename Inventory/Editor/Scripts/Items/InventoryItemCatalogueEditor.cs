using Celeste.Inventory;
using CelesteEditor.DataStructures;
using UnityEditor;

namespace CelesteEditor.Inventory
{
    [CustomEditor(typeof(InventoryItemCatalogue))]
    public class InventoryItemCatalogueEditor : IIndexableItemsEditor<InventoryItem>
    {
    }
}
