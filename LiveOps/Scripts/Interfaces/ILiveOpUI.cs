using Celeste.Assets;
using Celeste.Components;
using UnityEngine;

namespace Celeste.LiveOps
{
    public interface ILiveOpUI
    {
        ILoadRequest<GameObject> LoadUI(Instance instance, ILiveOpAssets assets, Transform parent);
    }

    public class NoLiveOpUI : ILiveOpUI
    {
        public ILoadRequest<GameObject> LoadUI(Instance instance, ILiveOpAssets assets, Transform parent)
        {
            return LoadAssetRequest<GameObject>.FromNothing();
        }
    }
}
