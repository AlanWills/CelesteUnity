using Celeste.Web.Messages;

namespace Celeste.Web
{
    public interface INetworkingClient : INetworkingObject
    {
        ulong Id { get; }
        
        void SendMessageToServer(string message);
        void SendMessageToServer<T>(NetworkingMessage<T> message);
    }
}