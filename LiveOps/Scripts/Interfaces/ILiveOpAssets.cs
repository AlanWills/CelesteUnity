using Celeste.Assets;
using Celeste.Components;
using System.Collections;
using UnityEngine;

namespace Celeste.LiveOps
{
    public interface ILiveOpAssets
    {
        bool IsLoaded { get; }

        IEnumerator LoadAssets(Instance instance);

        ILoadRequest<GameObject> LoadAsync(string key);
        ILoadRequest<GameObject> InstantiateAsync(string key, Transform parent);
    }

    public class NoLiveOpAssets : ILiveOpAssets
    {
        public bool IsLoaded => false;

        public IEnumerator LoadAssets(Instance instance)
        {
            yield break;
        }

        public ILoadRequest<GameObject> LoadAsync(string key)
        {
            return LoadAssetRequest<GameObject>.FromNothing();
        }

        public ILoadRequest<GameObject> InstantiateAsync(string key, Transform parent)
        {
            return LoadAssetRequest<GameObject>.FromNothing();
        }
    }
}
