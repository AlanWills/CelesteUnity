using Celeste.Components;

namespace Celeste.LiveOps
{
    public interface ILiveOpTimer
    {
        void SetStartTimestamp(Instance instance, long startTimestamp);
        long GetEndTimestamp(Instance instance);
    }

    public class NoLiveOpTimer : Component, ILiveOpTimer
    {
        public long GetEndTimestamp(Instance instance)
        {
            return 0L;
        }

        public void SetStartTimestamp(Instance instance, long startTimestamp) { }
    }
}
