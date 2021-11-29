using Celeste.Twine.Persistence;
using System.IO;
using UnityEditor;
using UnityEngine;
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
            DeletePersistentDataFile(TwinePersistence.FILE_NAME);
        }
    }
}