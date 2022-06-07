using Celeste.Persistence;
using Celeste.Scene;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CelesteEditor.Scene
{
    public static class MenuItemUtility
    {
        public static void LoadSceneSetMenuItem(string sceneSetPath)
        {
            SceneSet sceneSet = AssetDatabase.LoadAssetAtPath<SceneSet>(sceneSetPath);
            UnityEngine.Debug.Assert(sceneSet != null, $"Could not find Scene Set at path {sceneSetPath}.");

            if (sceneSet != null)
            {
                sceneSet.EditorOnly_Load(LoadSceneMode.Single);
            }
        }

        public static void OpenExplorerAt(string filePath)
        {
            System.Diagnostics.Process.Start("explorer.exe", filePath.Replace('/', '\\'));
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
                UnityEngine.Debug.Log($"Deleted save file at {filePath}.");
            }
        }
    }
}