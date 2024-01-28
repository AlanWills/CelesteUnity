using CelesteEditor.Persistence;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
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

        [MenuItem("Assets/Embed Package", false, 1000000)]
        private static void EmbedPackageMenuItem()
        {
            EmbedPackage.Embed(Selection.activeObject);
        }

        [MenuItem("Assets/Embed Package", true)]
        private static bool EmbedPackageValidation()
        {
            return EmbedPackage.CanEmbed(Selection.activeObject);
        }
    }
}
