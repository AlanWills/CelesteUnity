using Celeste.Assets;
using Celeste.Components;
using System.Collections;
using UnityEngine;

namespace Celeste.LiveOps
{
    public interface ILiveOpAssets
    {
        bool IsLoaded { get; }

        IEnumerator Load(Instance instance);

        ILoadRequest<GameObject> InstantiateAsync(string key, Transform parent);
    }

    public class NoLiveOpAssets : ILiveOpAssets
    {
        public bool IsLoaded => false;

        public IEnumerator Load(Instance instance)
        {
            yield break;
        }

        public ILoadRequest<GameObject> InstantiateAsync(string key, Transform parent)
        {
            return LoadAddressableRequest<GameObject>.FromNothing();
        }
    }
}
