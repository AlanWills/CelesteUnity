using Celeste.Scene;
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
    }
}