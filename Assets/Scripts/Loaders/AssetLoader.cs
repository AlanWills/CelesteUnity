using System.Collections;
using UnityEngine;

namespace Celeste.Assets
{
    [AddComponentMenu("Celeste/Assets/Asset Loader")]
    public class AssetLoader : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private bool parallelLoad = true;
        [SerializeField] private GameObject[] assetsToLoad = default;

        #endregion

        public void LoadAssets(AssetLoadingHandle assetLoadingHandle)
        {
            if (parallelLoad)
            {
                LoadAssetsParallel(assetLoadingHandle);
            }
            else
            {
                StartCoroutine(LoadAssetsSerial(assetLoadingHandle));
            }
        }

        private void LoadAssetsParallel(AssetLoadingHandle assetLoadingHandle)
        {
            foreach (var assetsToLoad in assetsToLoad)
            {
                StartCoroutine(LoadAssets(assetsToLoad, assetLoadingHandle));
            }
        }

        private IEnumerator LoadAssetsSerial(AssetLoadingHandle assetLoadingHandle)
        {
            foreach (var assetsToLoad in assetsToLoad)
            {
                yield return LoadAssets(assetsToLoad, assetLoadingHandle);
            }
        }

        IEnumerator LoadAssets(GameObject assetsToLoad, AssetLoadingHandle assetLoadingHandle)
        {
            using (assetLoadingHandle.Use())
            {
                IHasAssets hasAssetsToLoad = assetsToLoad.GetComponent<IHasAssets>();
                UnityEngine.Debug.Assert(hasAssetsToLoad != null, $"No {nameof(IHasAssets)} component found on {assetsToLoad.name} in {name}.", assetsToLoad);

                if (hasAssetsToLoad != null)
                {
                    yield return hasAssetsToLoad.LoadAssets();
                }
            }
        }

        public static AssetLoadingHandle LoadAllInScene(UnityEngine.SceneManagement.Scene scene)
        {
            AssetLoadingHandle assetLoadingHandle = new AssetLoadingHandle();

            if (scene.IsValid())
            {
                // Go through each root game object, to spare a whole hierarchy crawl
                // See if it has an asset loader and get it to load it's assets
                foreach (var rootGameObject in scene.GetRootGameObjects())
                {
                    AssetLoader assetLoader = rootGameObject.GetComponent<AssetLoader>();
                    if (assetLoader != null)
                    {
                        assetLoader.LoadAssets(assetLoadingHandle);
                    }
                }
            }
            else
            {
                UnityEngine.Debug.LogError($"Invalid scene {scene.name} passed into {nameof(LoadAllInScene)}.");
            }

            return assetLoadingHandle;
        }
    }
}