﻿using Celeste.Scene;
using UnityEditor;
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