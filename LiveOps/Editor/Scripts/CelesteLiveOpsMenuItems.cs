using Celeste.LiveOps.Persistence;
using CelesteEditor.Scene;
using UnityEditor;

namespace CelesteEditor.LiveOps
{
    public static class MenuItems
    {
        [MenuItem("Celeste/Save/Open Live Ops Save", priority = 0)]
        public static void OpenLiveOpsSaveMenuItem()
        {
            MenuItemUtility.OpenExplorerAtPersistentData();
        }

        [MenuItem("Celeste/Save/Delete Live Ops Save", priority = 100)]
        public static void DeleteLiveOpsSaveMenuItem()
        {
            MenuItemUtility.DeletePersistentDataFile(LiveOpsManager.FILE_NAME);
        }
    }
}