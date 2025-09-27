namespace Celeste.Web
{
    public interface INetworkingMessageReceiver
    {
        void OnNetworkingMessageReceived(string rawMessage);
    }
}