using Celeste.Components;

namespace Celeste.LiveOps
{
    public interface ILiveOpProgress
    {
        bool HasProgress(Instance instance);
    }

    public class NoLiveOpProgress : Component, ILiveOpProgress
    {
        public bool HasProgress(Instance instance) { return false; }
    }
}
