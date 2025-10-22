namespace Celeste.Web
{
    public interface IRawMessageNetworkHandler : INetworkMessageHandler
    {
        void OnMessageReceived(string rawMessage);
    }
}