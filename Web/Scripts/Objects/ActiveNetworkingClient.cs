#if USE_NETCODE
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
        private readonly INetworkingMessageDeserializer deserializer;
        private readonly INetworkingMessageHandler handler;

        #endregion

        public ActiveNetworkingClient(
            ulong clientId, 
            NetworkObject networkObject, 
            NetworkMessaging networkMessaging,
            INetworkingMessageSerializer serializer,
            INetworkingMessageDeserializer deserializer,
            INetworkingMessageHandler handler)
        {
            Id = clientId;
            this.networkObject = networkObject;
            this.networkMessaging = networkMessaging;
            this.serializer = serializer;
            this.deserializer = deserializer;
            this.handler = handler;

            networkMessaging.Setup(this);
        }
        
        public void SendMessageToServer(string message)
        {
            networkMessaging.SendMessageToServer(message);
        }

        public void SendMessageToServer<T>(NetworkingMessage<T> message)
        {
            string messageAsString = Serialize(message);
            SendMessageToServer(messageAsString);
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
                UnityEngine.Debug.LogError($"Failed to handle message '{message.Id}' on Client '{Id}'.  Is message valid? {message.IsValid}.", CelesteLog.Web);
            }
        }
        
        #endregion
    }
}
#endif