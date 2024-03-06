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

        public static void OpenPersistentFile(string fileName)
        {
            OpenWithDefaultApp(Path.Combine(Application.persistentDataPath, fileName));
        }
    }
}