using Celeste.Assets;
using Celeste.Components;
using UnityEngine;

namespace Celeste.LiveOps
{
    public interface ILiveOpWidget
    {
        ILoadRequest<GameObject> LoadWidget(Instance instance, ILiveOpAssets assets, Transform parent);
    }

    public class NoLiveOpWidget : Celeste.Components.Component, ILiveOpWidget
    {
        public ILoadRequest<GameObject> LoadWidget(Instance instance, ILiveOpAssets assets, Transform parent)
        {
            return LoadAssetRequest<GameObject>.FromNothing();
        }
    }
}
