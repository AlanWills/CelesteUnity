using System;
using System.Collections.Generic;

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

        public void DisconnectClient(ulong clientId)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Not Disconnecting Client {clientId}.", CelesteLog.Web);
        }

        public void OnMessageReceived(string rawMessage)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Discarding message: {rawMessage}.", CelesteLog.Web);
        }

        public void Shutdown()
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Ignoring {nameof(Shutdown)}.", CelesteLog.Web);
        }

        public void AddOnClientConnectedCallback(Action<INetworkingClient> onClientConnected)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Ignoring {nameof(AddOnClientConnectedCallback)}.", CelesteLog.Web);
        }

        public void RemoveOnClientConnectedCallback(Action<INetworkingClient> onClientConnected)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Ignoring {nameof(RemoveOnClientConnectedCallback)}.", CelesteLog.Web);
        }

        public void AddOnClientDisconnectedCallback(Action<ulong> clientId)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Ignoring {nameof(AddOnClientDisconnectedCallback)}.", CelesteLog.Web);
        }

        public void RemoveOnClientDisconnectedCallback(Action<ulong> clientId)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Ignoring {nameof(RemoveOnClientDisconnectedCallback)}.", CelesteLog.Web);
        }
    }
}