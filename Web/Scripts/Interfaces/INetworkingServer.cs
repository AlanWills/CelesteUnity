using System.Collections.Generic;
using Celeste.Web.Messages;

namespace Celeste.Web
{
    public interface INetworkingServer : INetworkingObject, INetworkingMessageReceiver
    {
        bool HasJoinCode { get; }
        string JoinCode { get; }
        bool HasConnectedClients { get; }
        IReadOnlyCollection<ulong> ConnectedClients { get; }

        void AddConnectedClient(ulong clientId);
        void RemoveConnectedClient(ulong clientId);
        
        void SendMessageToAllClients(string message);
        void SendMessageToAllClients<T>(NetworkingMessage<T> message);
        
        void SendMessageToClients(string message, IReadOnlyList<ulong> clientIds);
        void SendMessageToClients<T>(NetworkingMessage<T> message, IReadOnlyList<ulong> clientIds);
        
        void SendMessageToClient(string message, ulong clientId);
        void SendMessageToClient<T>(NetworkingMessage<T> message, ulong clientId);
    }
}