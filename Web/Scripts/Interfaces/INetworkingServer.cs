using System.Collections.Generic;
using Celeste.Web.Messages;
using Celeste.Web.Objects;

namespace Celeste.Web
{
    public interface INetworkingServer : INetworkingObject, INetworkingMessageReceiver
    {
        bool HasJoinCode { get; }
        string JoinCode { get; }
        bool HasConnectedClients { get; }
        IReadOnlyCollection<KeyValuePair<ulong, ActiveNetworkingClient>> ConnectedClients { get; }
        IEnumerable<ulong> ConnectedClientIds { get; }

        void AddConnectedClient(ActiveNetworkingClient networkingClient);
        void RemoveConnectedClient(ulong clientId);
        
        void SendMessageToAllClients<T>(NetworkingMessage<T> message);
        void SendMessageToClients<T>(NetworkingMessage<T> message, IReadOnlyList<ulong> clientIds);
        void SendMessageToClient<T>(NetworkingMessage<T> message, ulong clientId);
    }
}