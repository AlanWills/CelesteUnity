using Celeste.Web.Messages;

namespace Celeste.Web
{
    public interface INetworkingClient : INetworkingObject, INetworkingMessageReceiver
    {
        ulong Id { get; }
        bool HasNetworkObject { get; }

        void PingServer(string message);
        void SendMessageToServer<T>(NetworkingMessage<T> message);
    }
}