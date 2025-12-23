using Celeste.Events;
using UnityEngine.Events;

namespace Celeste.LiveOps
{
    public interface ILiveOpRecord
    {
        bool IsEnabled { get; }
        LiveOpState State { get; }
        LiveOp LiveOp { get; }
        
        IReadOnlyEvent<ValueChangedArgs<bool>> IsEnabledChanged { get; }
        IReadOnlyEvent DataChanged { get; }

        void Enable(LiveOp liveOp);
        void Disable();

        void CheckState();

        void Finish();
        void Complete();
    }
}
