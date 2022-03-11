using Celeste.Features.Persistence;
using UnityEditor;
using static CelesteEditor.Scene.MenuItemUtility;

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
            DeletePersistentDataFile(FeatureManager.FILE_NAME);
        }
    }
}