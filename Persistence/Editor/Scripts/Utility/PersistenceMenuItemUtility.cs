using System.IO;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Persistence
{
    public static class PersistenceMenuItemUtility
    {
        public static void OpenWithDefaultApp(string filePath)
        {
            if (!Application.isBatchMode)
            {
#if UNITY_EDITOR_WIN
                filePath = filePath.Replace('/', '\\');
#endif
                EditorUtility.OpenWithDefaultApp(filePath);
            }
        }

        public static void OpenExplorerAtPersistentData()
        {
            OpenWithDefaultApp(Application.persistentDataPath);
        }
        
        public static void OpenFolderInPersistentData(string relativeFolderPath)
        {
            string folderPath = Path.Combine(Application.persistentDataPath, relativeFolderPath);
            OpenWithDefaultApp(folderPath);
        }

        public static void OpenFileInPersistentData(string relativeFilePath)
        {
            string folderPath = Path.Combine(Application.persistentDataPath, relativeFilePath);
            OpenWithDefaultApp(folderPath);
        }
    }
}