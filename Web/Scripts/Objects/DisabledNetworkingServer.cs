using System;
using System.Collections.Generic;
using Celeste.Web.Messages;

namespace Celeste.Web.Objects
{
    public class DisabledNetworkingServer : INetworkingServer
    {
        public bool Exists => false;
        public bool HasNetworkObject => false;
        public bool HasJoinCode => false;
        public string JoinCode => string.Empty;
        public bool HasConnectedClients => false;
        public IReadOnlyCollection<ulong> ConnectedClients { get; } = Array.Empty<ulong>();
        
        public void AddConnectedClient(ulong clientId)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Not Adding Connected Client {clientId}.");
        }

        public void RemoveConnectedClient(ulong clientId)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Not Removing Connected Client {clientId}.");
        }

        public void SendMessageToAllClients(string message)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Discarding message: {message}.");
        }

        public void SendMessageToAllClients<T>(NetworkingMessage<T> message)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Discarding message: {message}.");
        }

        public void SendMessageToClients(string message, IReadOnlyList<ulong> clientIds)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Discarding message: {message}.");
        }

        public void SendMessageToClients<T>(NetworkingMessage<T> message, IReadOnlyList<ulong> clientIds)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Discarding message: {message}.");
        }

        public void SendMessageToClient(string message, ulong clientIds)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Discarding message: {message}.");
        }

        public void SendMessageToClient<T>(NetworkingMessage<T> message, ulong clientIds)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Discarding message: {message}.");
        }
    }
}