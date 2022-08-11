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
    }
}