using Celeste.Twine;
using UnityEditor;
using static CelesteEditor.Scene.MenuItemUtility;

namespace DnDEditor.Twine
{
    public static class MenuItems
    {
        [MenuItem("Celeste/Save/Open Twine Save", priority = 0)]
        public static void OpenTwineSaveMenuItem()
        {
            OpenExplorerAtPersistentData();
        }

        [MenuItem("Celeste/Save/Delete Twine Save", priority = 100)]
        public static void DeleteTwineSaveMenuItem()
        {
            DeletePersistentDataFile(TwineManager.FILE_NAME);
        }
    }
}