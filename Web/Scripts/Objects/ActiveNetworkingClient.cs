using Celeste.Web.Messages;
using Unity.Netcode;
using UnityEngine;

namespace Celeste.Web.Objects
{
    public class ActiveNetworkingClient : INetworkingClient
    {
        #region Properties and Fields
        
        public bool Exists => true;
        public bool HasNetworkObject => networkObject != null && networkMessaging != null;
        public ulong Id { get; }

        private readonly NetworkObject networkObject;
        private readonly NetworkMessaging networkMessaging;
        private readonly INetworkingMessageSerializer serializer;
        private readonly INetworkMessageDeserializer deserializer;

        #endregion

        public ActiveNetworkingClient(
            ulong clientId, 
            NetworkObject networkObject, 
            NetworkMessaging networkMessaging,
            INetworkingMessageSerializer serializer,
            INetworkMessageDeserializer deserializer)
        {
            Id = clientId;
            this.networkObject = networkObject;
            this.networkMessaging = networkMessaging;
            this.serializer = serializer;
            this.deserializer = deserializer;
        }
        
        public void SendMessageToServer(string message)
        {
            networkMessaging.SendMessageToServer(message);
        }

        public void SendMessageToServer<T>(NetworkingMessage<T> message)
        {
            string messageAsString = JsonUtility.ToJson(message);
            SendMessageToServer(messageAsString);
        }
    }
}