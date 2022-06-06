using Celeste.Components;

namespace Celeste.LiveOps
{
    public interface ILiveOpScheduleCondition
    {
        bool CanSchedule(Instance instance, InterfaceHandle<ILiveOpAssets> assets);
    }
}
