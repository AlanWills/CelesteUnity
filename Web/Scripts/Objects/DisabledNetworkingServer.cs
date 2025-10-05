using System;
using System.Collections.Generic;
using Celeste.Web.Messages;

namespace Celeste.Web.Objects
{
    public class DisabledNetworkingServer : INetworkingServer
    {
        public bool Exists => false;
        public bool HasJoinCode => false;
        public string JoinCode => string.Empty;
        public bool HasConnectedClients => false;
        public IReadOnlyDictionary<ulong, INetworkingClient> ConnectedClients { get; } = new Dictionary<ulong, INetworkingClient>();

        public void AddConnectedClient(INetworkingClient client)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Not Adding Connected Client {client.Id}.", CelesteLog.Web);
        }

        public void RemoveConnectedClient(ulong clientId)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Not Removing Connected Client {clientId}.", CelesteLog.Web);
        }

        public void SendMessageToAllClients<T>(NetworkingMessage<T> message)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Discarding message: {message}.", CelesteLog.Web);
        }

        public void SendMessageToClients<T>(NetworkingMessage<T> message, IReadOnlyList<ulong> clientIds)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Discarding message: {message}.", CelesteLog.Web);
        }

        public void SendMessageToClient<T>(NetworkingMessage<T> message, ulong clientIds)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Discarding message: {message}.", CelesteLog.Web);
        }

        public void OnNetworkingMessageReceived(string rawMessage)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Discarding message: {rawMessage}.", CelesteLog.Web);
        }

        public void AddOnClientConnectedCallback(Action<INetworkingClient> onClientConnected)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Ignoring {nameof(AddOnClientConnectedCallback)}.", CelesteLog.Web);
        }

        public void RemoveOnClientConnectedCallback(Action<INetworkingClient> onClientConnected)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Ignoring {nameof(RemoveOnClientConnectedCallback)}.", CelesteLog.Web);
        }
    }
}