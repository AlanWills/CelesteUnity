using Celeste.Options;
using Celeste.Persistence;
using UnityEditor;
using static CelesteEditor.Persistence.PersistenceMenuItemUtility;

namespace CelesteEditor.Options
{
    public static class CelesteOptionsMenuItems
    {
        [MenuItem("Celeste/Save/Files/Open Options Save", priority = 0)]
        public static void OpenOptionsSaveMenuItem()
        {
            OpenExplorerAtPersistentData();
        }

        [MenuItem("Celeste/Save/Files/Delete Options Save", priority = 100)]
        public static void DeleteOptionsSaveMenuItem()
        {
            PersistenceUtility.DeletePersistentDataFile(OptionsManager.FILE_NAME);
        }
    }
}