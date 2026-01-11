using Celeste.Persistence;
using Celeste.RemoteConfig;
using CelesteEditor.Persistence;
using UnityEditor;

namespace CelesteEditor.RemoteConfig
{
    public static class RemoteConfigPersistencePersistenceMenuItems
    {
        [MenuItem("Celeste/Save/Files/Open Remote Config Save", priority = 0)]
        public static void OpenGameRulesPersistenceSaveMenuItem()
        {
            PersistenceMenuItemUtility.OpenExplorerAtPersistentData();
        }

        [MenuItem("Celeste/Save/Files/Delete Remote Config Save", priority = 100)]
        public static void DeleteGameRulesPersistenceSaveMenuItem()
        {
            PersistenceUtility.DeletePersistentDataFile(RemoteConfigManager.FILE_NAME);
        }
    }
}
