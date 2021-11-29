using Celeste.Inventory.Persistence;
using CelesteEditor.Scene;
using UnityEditor;

namespace CelesteEditor.Inventory
{
    public static class MenuItems
    {
        [MenuItem("Celeste/Save/Open Inventory Save", priority = 0)]
        public static void OpenInventorySaveMenuItem()
        {
            MenuItemUtility.OpenExplorerAtPersistentData();
        }

        [MenuItem("Celeste/Save/Delete Inventory Save", priority = 100)]
        public static void DeleteInventorySaveMenuItem()
        {
            MenuItemUtility.DeletePersistentDataFile(InventoryPersistence.FILE_NAME);
        }
    }
}