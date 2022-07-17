namespace Celeste.LiveOps
{
    public interface ILiveOpRecord
    {
        bool IsEnabled { get; }

        void Enable(LiveOp liveOp);
        void Disable();

        void CheckIfFinished(LiveOp liveOp);

        void Finish();
        void Complete();
    }
}
