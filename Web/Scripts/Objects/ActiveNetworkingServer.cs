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
        public IReadOnlyDictionary<ulong, INetworkingClient> ConnectedClients => connectedClients;

        private readonly Dictionary<ulong, INetworkingClient> connectedClients = new();
        private readonly INetworkingMessageDeserializer deserializer;
        private readonly INetworkingMessageHandler handler;
        private Action<INetworkingClient> onClientConnected;

        #endregion

        public ActiveNetworkingServer(
            string joinCode,
            INetworkingMessageDeserializer deserializer,
            INetworkingMessageHandler handler)
        {
            JoinCode = joinCode;
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
            
            onClientConnected?.Invoke(client);
        }

        public void RemoveConnectedClient(ulong clientId)
        {
            connectedClients.Remove(clientId);
        }

        public void OnMessageReceived(string rawMessage)
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