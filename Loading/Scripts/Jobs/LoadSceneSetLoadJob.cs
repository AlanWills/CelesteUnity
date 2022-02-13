using Celeste.Assets;
using Celeste.Coroutines;
using Celeste.Scene;
using Celeste.Scene.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(LoadSceneSetLoadJob), menuName = "Celeste/Loading/Load Jobs/Load Scene Set")]
    public class LoadSceneSetLoadJob : LoadJob
    {
        #region Properties and Fields

        [SerializeField] private SceneSet sceneSet;
        [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Single;
        [SerializeField] private bool unloadUnusedAssets = true;

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            yield return sceneSet.LoadAsync(loadSceneMode, setProgress, () => { });

            LoadAssetsHandle handle = new LoadAssetsHandle();
            for (int i = 0, n = SceneManager.sceneCount; i < n; ++i)
            {
                yield return LoadAssets(SceneManager.GetSceneAt(i), handle);
            }

            if (unloadUnusedAssets)
            {
                yield return Resources.UnloadUnusedAssets();
            }
        }

        private IEnumerator LoadAssets(UnityEngine.SceneManagement.Scene scene, LoadAssetsHandle handle)
        {
            foreach (GameObject rootGameObject in scene.GetRootGameObjects())
            {
                foreach (IHasAssets hasAssets in rootGameObject.GetComponentsInChildren<IHasAssets>())
                {
                    if (hasAssets.ShouldLoadAssets())
                    {
                        CoroutineManager.Instance.StartCoroutine(LoadAssets(hasAssets, handle));
                    }
                }
            }

            yield return new WaitUntil(() => handle.LoadAssetsCount <= 0);
        }

        private IEnumerator LoadAssets(IHasAssets hasAssets, LoadAssetsHandle handle)
        {
            ++handle.LoadAssetsCount;
            
            yield return hasAssets.LoadAssets();

            --handle.LoadAssetsCount;
        }
    }
}