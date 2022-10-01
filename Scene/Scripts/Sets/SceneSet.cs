using Celeste.Assets;
using Celeste.DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityScene = UnityEngine.SceneManagement.Scene;

namespace Celeste.Scene
{
    [Serializable]
    public enum SceneType
    {
        Baked,
        Addressable
    }

    [Serializable]
    public struct SceneSetEntry
    {
        public SceneType sceneType;
        public string sceneId;

        public SceneSetEntry(string sceneId, SceneType sceneType)
        {
            this.sceneId = sceneId;
            this.sceneType = sceneType;
        }
    }

    [CreateAssetMenu(fileName = nameof(SceneSet), menuName = "Celeste/Scene/Scene Set")]
    public class SceneSet : ScriptableObject
    {
        #region Properties and Fields

        public int NumScenes
        {
            get { return scenes.Count; }
        }

        [SerializeField] private List<SceneSetEntry> scenes;

        #endregion

        public void AddScene(string sceneId, SceneType sceneType)
        {
            scenes.Add(new SceneSetEntry(sceneId, sceneType));
        }

        public string GetSceneId(int index)
        {
            return scenes.Get(index).sceneId;
        }

        public SceneType GetSceneType(int index)
        {
            return scenes.Get(index).sceneType;
        }

        public IEnumerator LoadAsync(LoadSceneMode loadSceneMode, Action<float> onProgressChanged, Action onLoadComplete)
        {
            List<UnityScene> scenesToUnload = new List<UnityScene>();
            HashSet<string> loadedScenes = new HashSet<string>();

            for (int i = 0, n = SceneManager.sceneCount; i < n; ++i)
            {
                UnityScene scene = SceneManager.GetSceneAt(i);
                if (loadSceneMode == LoadSceneMode.Single && !IsRequired(scene))
                {
                    scenesToUnload.Add(scene);
                }
                else
                {
                    loadedScenes.Add(scene.name);
                }
            }

            int totalScenesToLoadOrUnload = scenesToUnload.Count + scenes.Count - loadedScenes.Count;
            float progressChunkPerScene = 1f / totalScenesToLoadOrUnload;
            float progress = 0;

            for (int i = 0, n = scenes.Count; i < n; ++i)
            {
                string sceneId = GetSceneId(i);
                SceneType sceneType = GetSceneType(i);

                if (!loadedScenes.Contains(sceneId))
                {
                    UnityEngine.Debug.Log($"Beginning to load scene {sceneId}.");
                    UnityScene loadedScene = default;

                    if (sceneType == SceneType.Baked)
                    {
                        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);
                        while (!asyncOperation.isDone)
                        {
                            onProgressChanged.Invoke(progress + progressChunkPerScene * asyncOperation.progress);
                            yield return null;
                        }

                        loadedScene = SceneManager.GetSceneByName(sceneId);
                    }
                    else if (sceneType == SceneType.Addressable)
                    {
                        AsyncOperationHandle<SceneInstance> asyncOperation = Addressables.LoadSceneAsync(sceneId, LoadSceneMode.Additive);
                        while (!asyncOperation.IsDone)
                        {
                            onProgressChanged.Invoke(progress + progressChunkPerScene * asyncOperation.PercentComplete);
                            yield return null;
                        }

                        loadedScene = asyncOperation.Result.Scene;
                    }
                    else
                    {
                        UnityEngine.Debug.LogError($"Unhandled scene type {sceneType}.");
                    }

                    if (loadedScene.IsValid())
                    {
                        yield return LoadAssets(loadedScene);
                        UnityEngine.Debug.Log($"Successfully loaded scene {loadedScene.name}.");
                    }
                    else
                    {
                        UnityEngine.Debug.LogError($"Failed to load scene {sceneId}.");
                    }

                    progress += progressChunkPerScene;
                    onProgressChanged.Invoke(progress);
                }
            }

            for (int i = 0, n = scenesToUnload.Count; i < n; ++i)
            {
                UnityEngine.Debug.Log($"Beginning to unload scene {scenesToUnload[i].name}.");
                AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(scenesToUnload[i]);
                while (!asyncOperation.isDone)
                {
                    onProgressChanged.Invoke(progress + progressChunkPerScene * asyncOperation.progress);
                    yield return null;
                }

                UnityEngine.Debug.Log($"Successfully unloaded scene {scenesToUnload[i].name}.");
                progress += progressChunkPerScene;
                onProgressChanged.Invoke(progress);
            }

#if UNITY_EDITOR
            EditorOnly_SortScenes();
#endif

            onLoadComplete.Invoke();
        }

        public bool IsRequired(UnityScene scene)
        {
            for (int i = 0, n = scenes.Count; i < n; ++i)
            {
                if (string.CompareOrdinal(scenes[i].sceneId, scene.name) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerator LoadAssets(UnityScene scene)
        {
            AssetLoadingHandle handle = AssetLoader.LoadAllInScene(scene);
            yield return new WaitUntil(() => handle.LoadAssetsCount <= 0);
        }

#if UNITY_EDITOR
        public void EditorOnly_SortScenes()
        {
            List<UnityScene> unityScenes = new List<UnityScene>(SceneManager.sceneCount);
            for (int i = 0, n = SceneManager.sceneCount; i < n; ++i)
            {
                unityScenes.Add(SceneManager.GetSceneAt(i));
            }

            for (int i = 0, n = scenes.Count; i < (n - 1); ++i)
            {
                UnityScene first = unityScenes.Find(x => string.CompareOrdinal(x.name, scenes[i].sceneId) == 0);

                for (int j = i + 1; j < n; ++j)
                {
                    UnityScene second = unityScenes.Find(x => x.name == scenes[j].sceneId);
                    UnityEditor.SceneManagement.EditorSceneManager.MoveSceneBefore(first, second);
                }
            }
        }
#endif
    }
}
