using Celeste.Persistence;
using Celeste.Scene;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Scene
{
    public static class MenuItemUtility
    {
        public static void LoadSceneSetMenuItem(string sceneSetPath)
        {
            SceneSet sceneSet = AssetDatabase.LoadAssetAtPath<SceneSet>(sceneSetPath);
            Debug.Assert(sceneSet != null, $"Could not find Scene Set at path {sceneSetPath}.");

            if (sceneSet != null)
            {
                sceneSet.EditorOnly_Load();
            }
        }

        public static void OpenExplorerAt(string filePath)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = new System.Diagnostics.ProcessStartInfo("explorer.exe", filePath.Replace('/', '\\'));
            p.Start();
        }

        public static void OpenExplorerAtPersistentData()
        {
            OpenExplorerAt(Application.persistentDataPath);
        }

        public static void DeletePersistentDataFile(string fileNameAndExtension)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileNameAndExtension);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
#if UNITY_EDITOR
                File.Delete($"{filePath}.{PersistenceConstants.DEBUG_FILE_EXTENSION}");
#endif
                Debug.Log($"Deleted save file at {filePath}.");
            }
        }
    }
}