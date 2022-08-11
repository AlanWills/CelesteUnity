using Celeste.Features.Persistence;
using Celeste.Persistence;
using UnityEditor;
using static CelesteEditor.Persistence.PersistenceMenuItemUtility;

namespace CelesteEditor.Twine
{
    public static class MenuItems
    {
        [MenuItem("Celeste/Save/Open Feature Save", priority = 0)]
        public static void OpenFeatureSaveMenuItem()
        {
            OpenExplorerAtPersistentData();
        }

        [MenuItem("Celeste/Save/Delete Feature Save", priority = 100)]
        public static void DeleteFeatureSaveMenuItem()
        {
            PersistenceUtility.DeletePersistentDataFile(FeatureManager.FILE_NAME);
        }
    }
}