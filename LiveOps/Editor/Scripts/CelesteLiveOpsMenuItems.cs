using Celeste.LiveOps.Persistence;
using Celeste.Persistence;
using CelesteEditor.Persistence;
using UnityEditor;

namespace CelesteEditor.LiveOps
{
    public static class MenuItems
    {
        [MenuItem("Celeste/Save/Open Live Ops Save", priority = 0)]
        public static void OpenLiveOpsSaveMenuItem()
        {
            PersistenceMenuItemUtility.OpenExplorerAtPersistentData();
        }

        [MenuItem("Celeste/Save/Delete Live Ops Save", priority = 100)]
        public static void DeleteLiveOpsSaveMenuItem()
        {
            PersistenceUtility.DeletePersistentDataFile(LiveOpsManager.FILE_NAME);
        }
    }
}