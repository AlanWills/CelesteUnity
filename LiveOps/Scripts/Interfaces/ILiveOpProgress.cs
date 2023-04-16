using Celeste.Components;

namespace Celeste.LiveOps
{
    public interface ILiveOpProgress
    {
        void ResetProgress(Instance instance);
        float ProgressRatio(Instance instance);
        bool PlayerActionRequired(Instance instance);
    }

    public class NoLiveOpProgress : ILiveOpProgress
    {
        public void ResetProgress(Instance instance) { }
        public float ProgressRatio(Instance instance) { return 0f; }
        public bool PlayerActionRequired(Instance instance) { return false; }
    }
}
