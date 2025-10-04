#if USE_NETCODE
using System;
using System.Collections.Generic;
using Celeste.Web.Messages;

namespace Celeste.Web.Objects
{
    public class ActiveNetworkingServer : INetworkingServer
    {
        #region Properties and Fields
        
        public bool Exists => true;
        public bool HasJoinCode => !string.IsNullOrEmpty(JoinCode);
        public string JoinCode { get; }
        public bool HasConnectedClients => connectedClients.Count > 0;
        public IReadOnlyCollection<KeyValuePair<ulong, INetworkingClient>> ConnectedClients => connectedClients;

        public IEnumerable<ulong> ConnectedClientIds
        {
            get
            {
                foreach (var connectedClient in connectedClients)
                {
                    yield return connectedClient.Key;
                }
            }
        }

        private readonly Dictionary<ulong, INetworkingClient> connectedClients = new();
        private readonly INetworkingMessageSerializer serializer;
        private readonly INetworkingMessageDeserializer deserializer;
        private readonly INetworkingMessageHandler handler;
        private Action<INetworkingClient> onClientConnected;

        #endregion

        public ActiveNetworkingServer(
            string joinCode,
            INetworkingMessageSerializer serializer,
            INetworkingMessageDeserializer deserializer,
            INetworkingMessageHandler handler)
        {
            JoinCode = joinCode;
            this.serializer = serializer;
            this.deserializer = deserializer;
            this.handler = handler;
        }

        public void AddConnectedClient(INetworkingClient client)
        {
            connectedClients[client.Id] = client;
            
            foreach (var messageHandler in client.NetworkMessageHandlers)
            {
                messageHandler.SetServer(this);
            }
        }

        public void RemoveConnectedClient(ulong clientId)
        {
            connectedClients.Remove(clientId);
        }

        public void SendMessageToAllClients<T>(NetworkingMessage<T> message)
        {
            string messageAsString = Serialize(message);
            foreach (var client in connectedClients)
            {
                client.Value.SendMessage(messageAsString);
            }
        }

        public void SendMessageToClients<T>(NetworkingMessage<T> message, IReadOnlyList<ulong> clientIds)
        {
            string messageAsString = Serialize(message);
            foreach (var clientId in clientIds)
            {
                if (connectedClients.TryGetValue(clientId, out INetworkingClient client))
                {
                    client.SendMessage(messageAsString);
                }
                else
                {
                    UnityEngine.Debug.LogError($"Failed to find Client with Id {clientId} registered with the Server.  Skipping sending message...");
                }
            }
        }

        public void SendMessageToClient<T>(NetworkingMessage<T> message, ulong clientId)
        {
            string messageAsString = Serialize(message);
            if (connectedClients.TryGetValue(clientId, out INetworkingClient client))
            {
                client.SendMessage(messageAsString);
            }
            else
            {
                UnityEngine.Debug.LogError($"Failed to find Client with Id {clientId} registered with the Server.  Skipping sending message...");
            }
        }

        private string Serialize<T>(NetworkingMessage<T> message)
        {
            return serializer.Serialize(message);
        }
        
        public void OnNetworkingMessageReceived(string rawMessage)
        {
            NetworkingMessage message = deserializer.Deserialize(rawMessage);

            if (handler.CanHandle(message))
            {
                handler.Handle(message);
            }
            else
            {
                UnityEngine.Debug.LogError($"Failed to handle message '{message.Id}' on Server.  Is message valid? {message.IsValid}.", CelesteLog.Web);
            }
        }

        public void AddOnClientConnectedCallback(Action<INetworkingClient> onClientConnectedCallback)
        {
            onClientConnected += onClientConnectedCallback;
        }

        public void RemoveOnClientConnectedCallback(Action<INetworkingClient> onClientConnectedCallback)
        {
            onClientConnected -= onClientConnectedCallback;
        }
    }
}
#endif