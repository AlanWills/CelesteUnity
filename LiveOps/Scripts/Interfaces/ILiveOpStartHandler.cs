using Celeste.Components;

namespace Celeste.LiveOps
{
    public interface ILiveOpStartHandler
    {
        void OnLiveOpStarted(Instance instance);
    }
}