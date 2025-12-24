using Celeste.Assets;
using Celeste.Components;
using UnityEngine;

namespace Celeste.LiveOps
{
    public interface ILiveOpWidget
    {
        bool ShouldSpawnWidget(Instance instance, LiveOpState state);
        ILoadRequest<GameObject> SpawnWidget(Instance instance, ILiveOpAssets assets, Transform parent);
    }

    public class NoLiveOpWidget : ILiveOpWidget
    {
        public bool ShouldSpawnWidget(Instance instance, LiveOpState state)
        {
            return false;
        }

        public ILoadRequest<GameObject> SpawnWidget(Instance instance, ILiveOpAssets assets, Transform parent)
        {
            return LoadAssetRequest<GameObject>.FromNothing();
        }
    }
}
