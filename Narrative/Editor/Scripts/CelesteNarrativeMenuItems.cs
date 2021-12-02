using Celeste.Narrative;
using UnityEditor;
using static CelesteEditor.Scene.MenuItemUtility;

namespace CelesteEditor.Narrative
{
    public static class MenuItems
    {
        [MenuItem("Celeste/Save/Open Narrative Save", priority = 0)]
        public static void OpenNarrativeSaveMenuItem()
        {
            OpenExplorerAtPersistentData();
        }

        [MenuItem("Celeste/Save/Delete Narrative Save", priority = 100)]
        public static void DeleteNarrativeSaveMenuItem()
        {
            DeletePersistentDataFile(NarrativeManager.FILE_NAME);
        }
    }
}