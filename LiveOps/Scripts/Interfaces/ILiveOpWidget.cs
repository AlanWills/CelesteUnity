using Celeste.Assets;
using Celeste.Components;
using UnityEngine;

namespace Celeste.LiveOps
{
    public interface ILiveOpWidget
    {
        bool ShouldSpawnWidget(Instance instance, LiveOpState state);
        ILoadRequest<GameObject> LoadWidget(Instance instance, ILiveOpAssets assets);
    }

    public class NoLiveOpWidget : ILiveOpWidget
    {
        public bool ShouldSpawnWidget(Instance instance, LiveOpState state)
        {
            return false;
        }

        public ILoadRequest<GameObject> LoadWidget(Instance instance, ILiveOpAssets assets)
        {
            return LoadAssetRequest<GameObject>.FromNothing();
        }
    }
}
