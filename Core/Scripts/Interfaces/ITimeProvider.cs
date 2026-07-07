namespace Celeste.Core.Interfaces
{
    public interface ITimeProvider
    {
        float Time { get; }
        float DeltaTime { get; }
    }
}