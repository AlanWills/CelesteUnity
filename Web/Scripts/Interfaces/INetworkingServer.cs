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
        IReadOnlyDictionary<ulong, INetworkingClient> ConnectedClients { get; }

        void AddConnectedClient(INetworkingClient networkingClient);
        void RemoveConnectedClient(ulong clientId);
        void OnMessageReceived(string rawMessage);
        
        void AddOnClientConnectedCallback(Action<INetworkingClient> onClientConnected);
        void RemoveOnClientConnectedCallback(Action<INetworkingClient> onClientConnected);

    }
}