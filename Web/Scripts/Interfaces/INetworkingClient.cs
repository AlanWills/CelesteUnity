using System.Collections.Generic;
using Celeste.Web.Messages;

namespace Celeste.Web
{
    public interface INetworkingClient : INetworkingObject
    {
        ulong Id { get; }
        bool HasNetworkObject { get; }
        IReadOnlyList<INetworkMessageHandler> NetworkMessageHandlers { get; }

        void PingServer(string message);
        void SendMessageToServer<T>(NetworkingMessage<T> message);
        
        void Ping(string message);
        void SendMessage(string message);

        T GetNetworkMessageHandler<T>() where T : INetworkMessageHandler;
    }
}