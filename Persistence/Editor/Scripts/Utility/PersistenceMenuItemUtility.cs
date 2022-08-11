using UnityEngine;

namespace CelesteEditor.Persistence
{
    public static class PersistenceMenuItemUtility
    {
        public static void OpenExplorerAt(string filePath)
        {
            if (!Application.isBatchMode)
            {
                System.Diagnostics.Process.Start("explorer.exe", filePath.Replace('/', '\\'));
            }
        }

        public static void OpenExplorerAtPersistentData()
        {
            OpenExplorerAt(Application.persistentDataPath);
        }
    }
}