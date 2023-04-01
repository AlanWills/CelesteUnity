using CelesteEditor.Persistence;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UnityProject
{
    public static class CelesteUnityProjectMenuItems
    {
        [MenuItem("Celeste/Save/Delete Save Folder", priority = -1, validate = true)]
        public static bool ValidateDeleteSaveDataFolderMenuItem()
        {
            return Directory.Exists(Application.persistentDataPath);
        }

        [MenuItem("Celeste/Save/Delete Save Folder", priority = -1, validate = false)]
        public static void DeleteSaveDataFolderMenuItem()
        {
            Directory.Delete(Application.persistentDataPath, true);
        }

        [MenuItem("Celeste/Save/Open Save Folder", priority = -2, validate = true)]
        public static bool ValidateOpenSaveDataFolderMenuItem()
        {
            return Directory.Exists(Application.persistentDataPath);
        }

        [MenuItem("Celeste/Save/Open Save Folder", priority = -2, validate = false)]
        public static void OpenSaveDataFolderMenuItem()
        {
            PersistenceMenuItemUtility.OpenExplorerAtPersistentData();
        }
    }
}
