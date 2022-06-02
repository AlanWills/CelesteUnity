using Celeste.Components;

namespace Celeste.LiveOps
{
    public interface ILiveOpProgress
    {
        float ProgressRatio(Instance instance);
    }

    public class NoLiveOpProgress : ILiveOpProgress
    {
        public float ProgressRatio(Instance instance) { return 0f; }
    }
}
