using Celeste.Scene;
using CelesteEditor.Tools;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityScene = UnityEngine.SceneManagement.Scene;

namespace CelesteEditor.Scene
{
    public static class SceneUtility
    {
        public static Dictionary<string, string> CreateScenePathLookup()
        {
            Dictionary<string, string> scenePathLookup = new Dictionary<string, string>();

            foreach (string sceneGuid in AssetDatabase.FindAssets($"t:{nameof(SceneAsset)}"))
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(sceneGuid);
                SceneAsset unityScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(assetPath);

                if (unityScene.IsAssetAddressable())
                {
                    AddressableAssetEntry unitySceneAddressableInfo = unityScene.GetAddressableInfo();
                    scenePathLookup[unityScene.name] = assetPath;
                }
                else
                {
                    scenePathLookup[unityScene.name] = assetPath;
                }
            }

            return scenePathLookup;
        }

        public static Dictionary<string, SceneAsset> CreateSceneAssetLookup()
        {
            Dictionary<string, SceneAsset> scenePathLookup = new Dictionary<string, SceneAsset>();

            foreach (string sceneGuid in AssetDatabase.FindAssets($"t:{nameof(SceneAsset)}"))
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(sceneGuid);
                SceneAsset unityScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(assetPath);
                scenePathLookup[unityScene.name] = unityScene;
            }

            return scenePathLookup;
        }

        public static void EditorOnly_Load(this SceneSet sceneSet, LoadSceneMode loadSceneMode)
        {
            if (Application.isPlaying)
            {
                UnityEngine.Debug.LogAssertion($"Unable to synchronously load {sceneSet.name} whilst the application is playing.  This function is for setting up editor scenes only; use LoadAsync instead.");
                return;
            }

            HashSet<string> loadedScenes = new HashSet<string>();
            Dictionary<string, string> scenePathLookup = CreateScenePathLookup();

            List<UnityScene> scenesToUnload = new List<UnityScene>();
            for (int i = SceneManager.sceneCount; i > 0; --i)
            {
                UnityScene scene = SceneManager.GetSceneAt(i - 1);
                if (loadSceneMode == LoadSceneMode.Single && !sceneSet.IsRequired(scene))
                {
                    scenesToUnload.Add(scene);
                }
                else
                {
                    loadedScenes.Add(scene.name);
                }
            }

            for (int i = 0, n = sceneSet.NumScenes; i < n; ++i)
            {
                string sceneId = sceneSet.GetSceneId(i);

                if (!loadedScenes.Contains(sceneId))
                {
                    UnityEngine.Debug.Assert(scenePathLookup.ContainsKey(sceneId), $"Could not find scene {sceneId} in lookup.");
                    EditorSceneManager.OpenScene(scenePathLookup[sceneId], OpenSceneMode.Additive);
                }
            }

            foreach (UnityScene scene in scenesToUnload)
            {
                EditorSceneManager.CloseScene(scene, true);
            }

            sceneSet.EditorOnly_SortScenes();
        }
    }
}
