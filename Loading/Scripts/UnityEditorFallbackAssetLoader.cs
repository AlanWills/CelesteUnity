using Celeste.Assets;
using Celeste.Parameters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celeste.Loading
{
    [AddComponentMenu("Celeste/Loading/Unity Editor Fallback Asset Loader")]
    public class UnityEditorFallbackAssetLoader : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private BoolValue shouldFallbackLoadAssets;

        private Coroutine fallbackLoadAssetsCoroutine;
        private List<Coroutine> loadAssetsCoroutine = new List<Coroutine>();

        #endregion

        #region Unity Methods

        private void Start()
        {
            if (shouldFallbackLoadAssets != null && shouldFallbackLoadAssets.Value)
            {
                fallbackLoadAssetsCoroutine = StartCoroutine(FallbackLoadAssets());
            }
        }

        private void OnDisable()
        {
            if (fallbackLoadAssetsCoroutine != null)
            {
                StopCoroutine(fallbackLoadAssetsCoroutine);
                fallbackLoadAssetsCoroutine = null;
            }

            if (loadAssetsCoroutine.Count > 0)
            {
                foreach (var loadAssetCoroutine in loadAssetsCoroutine)
                {
                    StopCoroutine(loadAssetCoroutine);
                }

                loadAssetsCoroutine.Clear();
            }
        }

        #endregion

        private IEnumerator FallbackLoadAssets()
        {
            LoadAssetsHandle handle = new LoadAssetsHandle();
            for (int i = 0, n = SceneManager.sceneCount; i < n; ++i)
            {
                yield return LoadAssets(SceneManager.GetSceneAt(i), handle);
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
                        loadAssetsCoroutine.Add(StartCoroutine(LoadAssets(hasAssets, handle)));
                    }
                }
            }

            yield return new WaitUntil(() => handle.LoadAssetsCount <= 0);

            loadAssetsCoroutine.Clear();
        }

        private IEnumerator LoadAssets(IHasAssets hasAssets, LoadAssetsHandle handle)
        {
            ++handle.LoadAssetsCount;

            yield return hasAssets.LoadAssets();

            --handle.LoadAssetsCount;
        }
    }
}