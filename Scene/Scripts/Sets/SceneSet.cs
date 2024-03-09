using Celeste.Assets;
using Celeste.DataStructures;
using Celeste.Debug.Settings;
using Celeste.Parameters;
using Celeste.Tools.Attributes.GUI;
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
        public bool isDebugOnly;

        public SceneSetEntry(string sceneId, SceneType sceneType, bool isDebugOnly)
        {
            this.sceneId = sceneId;
            this.sceneType = sceneType;
            this.isDebugOnly = isDebugOnly;
        }

        public bool ShouldLoad(bool isDebug)
        {
            return !isDebugOnly || isDebug;
        }
    }

    [CreateAssetMenu(fileName = nameof(SceneSet), order = CelesteMenuItemConstants.SCENE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SCENE_MENU_ITEM + "Scene Set")]
    public class SceneSet : ScriptableObject
    {
        #region Properties and Fields

        public int NumScenes => scenes.Count;

        public bool HasCustomDebugBuildValue
        {
            get => hasCustomDebugBuildValue;
            set
            {
                if (hasCustomDebugBuildValue != value)
                {
                    hasCustomDebugBuildValue = value;
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
                }
            }
        }

        private bool IsDebug
        {
            get
            {
                if (hasCustomDebugBuildValue && isDebugBuild != null)
                {
                    return isDebugBuild.Value;
                }

                return UnityEngine.Debug.isDebugBuild;
            }
        }

        [SerializeField] private List<SceneSetEntry> scenes = new List<SceneSetEntry>();

        [Header("Loading Settings")]
        [SerializeField] private bool unloadResourcesOnLoad = true;

        [Header("Debug Settings")]
        [SerializeField] private bool hasCustomDebugBuildValue = true;
        [SerializeField, ShowIf(nameof(hasCustomDebugBuildValue))] private BoolValue isDebugBuild;
        [SerializeField] private bool checkForMissingComponents = true;

        #endregion

        #region Unity Methods

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (hasCustomDebugBuildValue)
            {
                if (isDebugBuild == null)
                {
                    isDebugBuild = DebugEditorSettings.GetOrCreateSettings().isDebugBuildValue;

                    if (isDebugBuild != null)
                    {
                        UnityEditor.EditorUtility.SetDirty(this);
                    }
                }
            }
            else
            {
                if (isDebugBuild != null)
                {
                    isDebugBuild = null;
                    UnityEditor.EditorUtility.SetDirty(this);
                }
            }
        }
#endif

        #endregion

        public void AddScene(string sceneId, SceneType sceneType, bool isDebugBuild)
        {
            if (!scenes.Exists(x => string.CompareOrdinal(x.sceneId, sceneId) == 0))
            {
                scenes.Add(new SceneSetEntry(sceneId, sceneType, isDebugBuild));
            }
        }

        public void MergeFrom(SceneSet targetSceneSet)
        {
            for (int i = 0, n = targetSceneSet.NumScenes; i < n; ++i)
            {
                AddSceneEntry(targetSceneSet.GetSceneEntry(i));
            }
        }

        public string GetSceneId(int index)
        {
            return scenes.Get(index).sceneId;
        }

        public SceneType GetSceneType(int index)
        {
            return scenes.Get(index).sceneType;
        }

        private void AddSceneEntry(SceneSetEntry sceneSetEntry)
        {
            AddScene(sceneSetEntry.sceneId, sceneSetEntry.sceneType, sceneSetEntry.isDebugOnly);
        }

        private SceneSetEntry GetSceneEntry(int index)
        {
            return scenes.Get(index);
        }

        public IEnumerator LoadAsync(
            LoadSceneMode loadSceneMode, 
            Action<float> onProgressChanged, 
            Action<string> setOutput, 
            Action onLoadComplete)
        {
            List<UnityScene> scenesToUnload = new List<UnityScene>();
            HashSet<string> loadedScenes = new HashSet<string>();

            for (int i = 0, n = SceneManager.sceneCount; i < n; ++i)
            {
                UnityScene scene = SceneManager.GetSceneAt(i);
                if (IsRequired(scene))
                {
                    loadedScenes.Add(scene.name);
                }
                else if (loadSceneMode == LoadSceneMode.Single)
                {
                    // In single load mode, we must remove any un-needed scenes, but in additive mode we don't care
                    // We can just leave anything that's already loaded be
                    scenesToUnload.Add(scene);
                }
            }

            int totalScenesToLoadOrUnload = scenesToUnload.Count + scenes.Count - loadedScenes.Count;
            float progressChunkPerScene = 1f / totalScenesToLoadOrUnload;
            UnityEngine.Debug.Assert(progressChunkPerScene > 0, $"Negative progress chunk for SceneSet loading.  This is undoubtedly a bug.");

            float progress = 0;

            for (int i = 0, n = scenes.Count; i < n; ++i)
            {
                var sceneSetScene = scenes[i];
                string sceneId = sceneSetScene.sceneId;
                SceneType sceneType = sceneSetScene.sceneType;

                if (sceneSetScene.ShouldLoad(IsDebug) && !loadedScenes.Contains(sceneId))
                {
                    setOutput($"Loading {sceneId}");
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

            if (unloadResourcesOnLoad)
            {
                UnityEngine.Debug.Log($"Beginning to unload unused assets.");
                yield return Resources.UnloadUnusedAssets();
                UnityEngine.Debug.Log($"Finished unloading unused assets.");
            }

            // Debug Actions
            if (IsDebug)
            {
                if (checkForMissingComponents)
                {
                    Tools.SceneUtility.FindMissingComponentsInLoadedScenes();
                }
            }

            onLoadComplete.Invoke();
        }

        public bool IsRequired(UnityScene scene)
        {
            for (int i = 0, n = scenes.Count; i < n; ++i)
            {
                var sceneSetScene = scenes[i];

                if (string.CompareOrdinal(sceneSetScene.sceneId, scene.name) == 0)
                {
                    return sceneSetScene.ShouldLoad(IsDebug);
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
