using Celeste.Inventory;
using Celeste.Persistence;
using UnityEditor;
using static CelesteEditor.Persistence.PersistenceMenuItemUtility;

namespace CelesteEditor.Inventory
{
    public static class MenuItems
    {
        [MenuItem("Celeste/Save/Open Inventory Save", priority = 0)]
        public static void OpenInventorySaveMenuItem()
        {
            OpenExplorerAtPersistentData();
        }

        [MenuItem("Celeste/Save/Delete Inventory Save", priority = 100)]
        public static void DeleteInventorySaveMenuItem()
        {
            PersistenceUtility.DeletePersistentDataFile(InventoryManager.FILE_NAME);
        }
    }
}