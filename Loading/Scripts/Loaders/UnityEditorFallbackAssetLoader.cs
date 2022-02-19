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
        }

        #endregion

        private IEnumerator FallbackLoadAssets()
        {
            for (int i = 0, n = SceneManager.sceneCount; i < n; ++i)
            {
                yield return LoadAssets(SceneManager.GetSceneAt(i));
            }
        }

        private IEnumerator LoadAssets(UnityEngine.SceneManagement.Scene scene)
        {
            AssetLoadingHandle handle = AssetLoader.LoadAllInScene(scene);
            yield return new WaitUntil(() => handle.LoadAssetsCount <= 0);
        }
    }
}