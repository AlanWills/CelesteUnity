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
    }
}
