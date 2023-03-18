using Celeste.Events;
using UnityEngine.Events;

namespace Celeste.LiveOps
{
    public interface ILiveOpRecord
    {
        bool IsEnabled { get; }
        LiveOpState State { get; }
        LiveOp LiveOp { get; }

        void Enable(LiveOp liveOp);
        void Disable();

        void CheckState();

        void Finish();
        void Complete();

        void AddIsEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback);
        void RemoveIsEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback);

        void AddDataChangedCallback(UnityAction callback);
        void RemoveDataChangedCallback(UnityAction callback);
    }
}
