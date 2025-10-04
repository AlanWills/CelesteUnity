using System;
using System.Collections.Generic;
using Celeste.Web.Messages;

namespace Celeste.Web
{
    public interface INetworkingServer : INetworkingObject
    {
        bool HasJoinCode { get; }
        string JoinCode { get; }
        bool HasConnectedClients { get; }
        IReadOnlyCollection<KeyValuePair<ulong, INetworkingClient>> ConnectedClients { get; }
        IEnumerable<ulong> ConnectedClientIds { get; }

        void AddConnectedClient(INetworkingClient networkingClient);
        void RemoveConnectedClient(ulong clientId);
        
        void SendMessageToAllClients<T>(NetworkingMessage<T> message);
        void SendMessageToClients<T>(NetworkingMessage<T> message, IReadOnlyList<ulong> clientIds);
        void SendMessageToClient<T>(NetworkingMessage<T> message, ulong clientId);
        
        void AddOnClientConnectedCallback(Action<INetworkingClient> onClientConnected);
        void RemoveOnClientConnectedCallback(Action<INetworkingClient> onClientConnected);
    }
}