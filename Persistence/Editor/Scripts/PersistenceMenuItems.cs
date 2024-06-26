using Celeste.Persistence;
using Celeste.Persistence.Snapshots;
using Celeste.Tools;
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Persistence
{
    public static class PersistenceMenuItems
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

        [MenuItem("Celeste/Tools/Load Data Snapshot")]
        public static void LoadDataSnapshotFromFile()
        {
            string filePath = EditorUtility.OpenFilePanel("Select Data Snapshot", "", "*");

            if (File.Exists(filePath))
            {
                string fileContents = File.ReadAllText(filePath);
                DataSnapshot dataSnapshot = ScriptableObject.CreateInstance<DataSnapshot>();
                JsonUtility.FromJsonOverwrite(fileContents, dataSnapshot);
                dataSnapshot.UnpackItems();
            }
        }
    }
}
