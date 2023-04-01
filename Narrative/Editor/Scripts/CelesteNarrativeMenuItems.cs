using Celeste.Narrative;
using Celeste.Persistence;
using UnityEditor;
using static CelesteEditor.Persistence.PersistenceMenuItemUtility;

namespace CelesteEditor.Narrative
{
    public static class MenuItems
    {
        [MenuItem("Celeste/Save/Files/Open Narrative Save", priority = 0)]
        public static void OpenNarrativeSaveMenuItem()
        {
            OpenExplorerAtPersistentData();
        }

        [MenuItem("Celeste/Save/Files/Delete Narrative Save", priority = 100)]
        public static void DeleteNarrativeSaveMenuItem()
        {
            PersistenceUtility.DeletePersistentDataFile(NarrativeManager.FILE_NAME);
        }
    }
}