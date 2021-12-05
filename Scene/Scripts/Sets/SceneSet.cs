using Celeste.DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityScene = UnityEngine.SceneManagement.Scene;

namespace Celeste.Scene
{
    [CreateAssetMenu(fileName = "SceneSet", menuName = "Celeste/Scene/Scene Set")]
    public class SceneSet : ScriptableObject
    {
        #region Properties and Fields

        public int NumScenes
        {
            get { return scenes.Count; }
        }

        [SerializeField] private List<string> scenes;

        #endregion

        public string GetSceneName(int index)
        {
            return scenes.Get(index);
        }

#if UNITY_EDITOR
        public void EditorOnly_Load()
        {
            if (Application.isPlaying)
            {
                UnityEngine.Debug.LogAssertion($"Unable to synchronously load {name} whilst the application is playing.  This function is for setting up editor scenes only; use LoadAsync instead.");
                return;
            }

            Dictionary<string, string> scenePathLookup = new Dictionary<string, string>();
            HashSet<string> loadedScenes = new HashSet<string>();

            foreach (string sceneGuid in UnityEditor.AssetDatabase.FindAssets($"t:{typeof(UnityEditor.SceneAsset).Name}"))
            {
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(sceneGuid);
                UnityEditor.SceneAsset sceneAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.SceneAsset>(assetPath);
                scenePathLookup[sceneAsset.name] = assetPath;
            }

            List<UnityScene> scenesToUnload = new List<UnityScene>();
            for (int i = SceneManager.sceneCount; i > 0; --i)
            {
                UnityScene scene = SceneManager.GetSceneAt(i - 1);
                if (!IsRequired(scene))
                {
                    scenesToUnload.Add(scene);
                }
                else
                {
                    loadedScenes.Add(scene.name);
                }
            }

            for (int i = 0, n = scenes.Count; i < n; ++i)
            {
                if (!loadedScenes.Contains(scenes[i]))
                {
                    UnityEngine.Debug.Assert(scenePathLookup.ContainsKey(scenes[i]), $"Could not find scene {scenes[i]} in lookup.");
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePathLookup[scenes[i]], UnityEditor.SceneManagement.OpenSceneMode.Additive);
                }
            }

            foreach (UnityScene scene in scenesToUnload)
            {
                UnityEditor.SceneManagement.EditorSceneManager.CloseScene(scene, true);
            }

            EditorOnly_SortScenes();
        }
#endif

        public IEnumerator LoadAsync(Action<float> onProgressChanged, Action onLoadComplete)
        {
            List<UnityScene> scenesToUnload = new List<UnityScene>();
            HashSet<string> loadedScenes = new HashSet<string>();

            for (int i = 0, n = SceneManager.sceneCount; i < n; ++i)
            {
                UnityScene scene = SceneManager.GetSceneAt(i);
                if (!IsRequired(scene))
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
                if (!loadedScenes.Contains(scenes[i]))
                {
                    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scenes[i], LoadSceneMode.Additive);
                    while (!asyncOperation.isDone)
                    {
                        onProgressChanged.Invoke(progress + progressChunkPerScene * asyncOperation.progress);
                        yield return null;
                    }

                    yield return new WaitForSeconds(4);

                    progress += progressChunkPerScene;
                    onProgressChanged.Invoke(progress);
                }
            }

            for (int i = 0, n = scenesToUnload.Count; i < n; ++i)
            {
                AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(scenesToUnload[i]);
                while (!asyncOperation.isDone)
                {
                    onProgressChanged.Invoke(progress + progressChunkPerScene * asyncOperation.progress);
                    yield return null;
                }

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
                if (string.CompareOrdinal(scenes[i], scene.name) == 0)
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_EDITOR
        private void EditorOnly_SortScenes()
        {
            List<UnityScene> unityScenes = new List<UnityScene>(SceneManager.sceneCount);
            for (int i = 0, n = SceneManager.sceneCount; i < n; ++i)
            {
                unityScenes.Add(SceneManager.GetSceneAt(i));
            }

            UnityEngine.Debug.Assert(scenes.Count == unityScenes.Count, "Number of loaded scenes did not match the number of scenes in the scene set");
            for (int i = 0, n = scenes.Count; i < (n - 1); ++i)
            {
                UnityScene first = unityScenes.Find(x => x.name == scenes[i]);

                for (int j = i + 1; j < n; ++j)
                {
                    UnityScene second = unityScenes.Find(x => x.name == scenes[j]);
                    UnityEditor.SceneManagement.EditorSceneManager.MoveSceneBefore(first, second);
                }
            }
        }
#endif
    }
}
