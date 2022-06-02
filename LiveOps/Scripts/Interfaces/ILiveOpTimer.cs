using Celeste.Components;

namespace Celeste.LiveOps
{
    public interface ILiveOpTimer
    {
        long GetEndTimestamp(Instance instance, long startTimestamp);
    }

    public class NoLiveOpTimer : ILiveOpTimer
    {
        public long GetEndTimestamp(Instance instance, long startTimestamp)
        {
            return 0L;
        }
    }
}
