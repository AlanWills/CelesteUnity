namespace Celeste.Web
{
    public interface INetworkingObject
    {
        bool Exists { get; }

        void OnNetworkingMessageReceived(string message);
    }
}