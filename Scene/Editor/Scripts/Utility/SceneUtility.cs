using Celeste.Scene;
using CelesteEditor.Tools;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityScene = UnityEngine.SceneManagement.Scene;

namespace CelesteEditor.Scene
{
    public static class SceneUtility
    {
        public struct SceneInfo : IEquatable<SceneInfo>
        {
            public string name;
            public bool isAddressable;

            public bool Equals(SceneInfo other)
            {
                return string.CompareOrdinal(name, other.name) == 0 && isAddressable == other.isAddressable;
            }

            public override bool Equals(object obj)
            {
                if (obj is SceneInfo sceneInfoObj)
                {
                    return Equals(sceneInfoObj);
                }

                return false;
            }

            public override int GetHashCode()
            {
                return name.GetHashCode() + isAddressable.GetHashCode();
            }

            public override string ToString()
            {
                return $"{name} {isAddressable}";
            }
        }

        public static Dictionary<SceneInfo, string> CreateScenePathLookup()
        {
            Dictionary<SceneInfo, string> scenePathLookup = new Dictionary<SceneInfo, string>();

            foreach (string sceneGuid in AssetDatabase.FindAssets($"t:{nameof(SceneAsset)}", new string[] { "Assets" }))
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(sceneGuid);
                SceneAsset unityScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(assetPath);
                SceneInfo sceneInfo = new SceneInfo()
                {
                    name = unityScene.name,
                    isAddressable = unityScene.IsAssetAddressable()
                };

                UnityEngine.Debug.Assert(!scenePathLookup.ContainsKey(sceneInfo), $"Scene Collision!!!!  Detected duplicate scenes with info '{sceneInfo}'.");
                scenePathLookup[sceneInfo] = assetPath;
            }

            return scenePathLookup;
        }

        public static Dictionary<string, SceneAsset> CreateSceneAssetLookup()
        {
            Dictionary<string, SceneAsset> scenePathLookup = new Dictionary<string, SceneAsset>();

            foreach (string sceneGuid in AssetDatabase.FindAssets($"t:{nameof(SceneAsset)}", new string[] { "Assets" }))
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
            Dictionary<SceneInfo, string> scenePathLookup = CreateScenePathLookup();

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
                SceneInfo sceneInfo = new SceneInfo()
                {
                    name = sceneSet.GetSceneId(i),
#if USE_ADDRESSABLES
                    isAddressable = sceneSet.GetSceneType(i) == SceneType.Addressable
#else
                    isAddressable = false
#endif
                };

                if (!loadedScenes.Contains(sceneInfo.name))
                {
                    UnityEngine.Debug.Assert(scenePathLookup.ContainsKey(sceneInfo), $"Could not find scene {sceneInfo.name} in lookup.");
                    EditorSceneManager.OpenScene(scenePathLookup[sceneInfo], OpenSceneMode.Additive);
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
