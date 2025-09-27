#if USE_NETCODE
using System.Collections.Generic;
using Celeste.Web.Messages;
using Unity.Netcode;
using UnityEngine;

namespace Celeste.Web.Objects
{
    public class ActiveNetworkingServer : INetworkingServer
    {
        #region Properties and Fields
        
        public bool Exists => true;
        public bool HasNetworkObject => networkObject != null && networkMessaging != null;
        public bool HasJoinCode => !string.IsNullOrEmpty(JoinCode);
        public string JoinCode { get; }
        public bool HasConnectedClients => connectedClients.Count > 0;
        public IReadOnlyCollection<ulong> ConnectedClients => connectedClients;

        private readonly HashSet<ulong> connectedClients = new();
        private readonly NetworkObject networkObject;
        private readonly NetworkMessaging networkMessaging;
        private readonly INetworkingMessageSerializer serializer;
        private readonly INetworkingMessageDeserializer deserializer;
        private readonly INetworkingMessageHandler handler;

        #endregion

        public ActiveNetworkingServer(
            string joinCode, 
            NetworkObject networkObject, 
            NetworkMessaging networkMessaging,
            INetworkingMessageSerializer serializer,
            INetworkingMessageDeserializer deserializer,
            INetworkingMessageHandler handler)
        {
            JoinCode = joinCode;
            this.networkObject = networkObject;
            this.networkMessaging = networkMessaging;
            this.serializer = serializer;
            this.deserializer = deserializer;
            this.handler = handler;
            
            networkMessaging.Setup(this);
        }

        public void AddConnectedClient(ulong clientId)
        {
            connectedClients.Add(clientId);
        }

        public void RemoveConnectedClient(ulong clientId)
        {
            connectedClients.Remove(clientId);
        }

        public void SendMessageToAllClients(string message)
        {
            networkMessaging.SendMessageToAllClients(message);
        }

        public void SendMessageToAllClients<T>(NetworkingMessage<T> message)
        {
            string messageAsString = Serialize(message);
            SendMessageToAllClients(messageAsString);
        }

        public void SendMessageToClients(string message, IReadOnlyList<ulong> clientIds)
        {
            networkMessaging.SendMessageToClients(message, clientIds);
        }

        public void SendMessageToClients<T>(NetworkingMessage<T> message, IReadOnlyList<ulong> clientIds)
        {
            string messageAsString = Serialize(message);
            SendMessageToClients(messageAsString, clientIds);
        }

        public void SendMessageToClient(string message, ulong clientId)
        {
            networkMessaging.SendMessageToClient(message, clientId);
        }

        public void SendMessageToClient<T>(NetworkingMessage<T> message, ulong clientId)
        {
            string messageAsString = Serialize(message);
            SendMessageToClient(messageAsString, clientId);
        }

        private string Serialize<T>(NetworkingMessage<T> message)
        {
            return serializer.Serialize(message);
        }
        
        #region INetworkingMessageReceiver

        void INetworkingMessageReceiver.OnNetworkingMessageReceived(string rawMessage)
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
        
        #endregion
    }
}
#endif