using System.Collections;
using UnityEngine;

namespace Celeste.Assets
{
    [AddComponentMenu("Celeste/Assets/Asset Loader")]
    public class AssetLoader : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GameObject[] assetsToLoad = default;

        #endregion

        public void LoadAssets(AssetLoadingHandle assetLoadingHandle)
        {
            foreach (var assetsToLoad in assetsToLoad)
            {
                StartCoroutine(LoadAssets(assetsToLoad, assetLoadingHandle));
            }
        }

        IEnumerator LoadAssets(GameObject assetsToLoad, AssetLoadingHandle assetLoadingHandle)
        {
            using (assetLoadingHandle.Use())
            {
                IHasAssets hasAssetsToLoad = assetsToLoad.GetComponent<IHasAssets>();
                Debug.Assert(hasAssetsToLoad != null, $"No {nameof(IHasAssets)} component found on {assetsToLoad.name} in {name}.", assetsToLoad);

                if (hasAssetsToLoad != null)
                {
                    yield return hasAssetsToLoad.LoadAssets();
                }
            }
        }

        public static AssetLoadingHandle LoadAllInScene(UnityEngine.SceneManagement.Scene scene)
        {
            AssetLoadingHandle assetLoadingHandle = new AssetLoadingHandle();

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

            return assetLoadingHandle;
        }
    }
}