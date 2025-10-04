#if USE_NETCODE
using Celeste.Web.Messages;

namespace Celeste.Web.Objects
{
    public class ActiveNetworkingClient : INetworkingClient
    {
        #region Properties and Fields
        
        public bool Exists => true;
        public bool HasNetworkObject => networkMessaging != null;
        public ulong Id { get; }
        
        private readonly INetworkingMessageSerializer serializer;
        private readonly INetworkingMessageDeserializer deserializer;
        private readonly INetworkingMessageHandler clientMessageHandler;
        private readonly NetworkMessaging networkMessaging;

        #endregion

        public ActiveNetworkingClient(
            ulong clientId,
            NetworkMessaging networkMessaging,
            INetworkingMessageSerializer serializer,
            INetworkingMessageDeserializer deserializer,
            INetworkingMessageHandler clientMessageHandler)
        {
            Id = clientId;
            this.networkMessaging = networkMessaging;
            this.serializer = serializer;
            this.deserializer = deserializer;
            this.clientMessageHandler = clientMessageHandler;
            
            networkMessaging.SetClientMessageReceiver(this); ;
        }

        public void SetServerMessageReceiver(INetworkingServer server)
        {
            networkMessaging.SetServerMessageReceiver(server);
        }

        public void Ping(string message)
        {
            UnityEngine.Debug.Assert(networkMessaging != null, $"Attempting to send a message without {nameof(NetworkMessaging)} being set!", CelesteLog.Web);
            networkMessaging?.PingClientRpc(message);
        }

        public void PingServer(string message)
        {
            UnityEngine.Debug.Assert(networkMessaging != null, $"Attempting to send a message without {nameof(NetworkMessaging)} being set!", CelesteLog.Web);
            networkMessaging?.PingServerRpc(message);
        }

        public void SendMessageToClient(string messageAsString)
        {
            UnityEngine.Debug.Assert(networkMessaging != null, $"Attempting to send a message without {nameof(NetworkMessaging)} being set!", CelesteLog.Web);
            networkMessaging?.SendMessageToClient(messageAsString, Id);
        }
        
        public void SendMessageToServer<T>(NetworkingMessage<T> message)
        {
            string messageAsString = Serialize(message);
            UnityEngine.Debug.Assert(networkMessaging != null, $"Attempting to send a message without {nameof(NetworkMessaging)} being set!", CelesteLog.Web);
            networkMessaging?.SendMessageToServerRpc(messageAsString);
        }

        private string Serialize<T>(NetworkingMessage<T> message)
        {
            return serializer.Serialize(message);
        }
        
        #region INetworkingMessageReceiver

        void INetworkingMessageReceiver.OnNetworkingMessageReceived(string rawMessage)
        {
            NetworkingMessage message = deserializer.Deserialize(rawMessage);

            if (clientMessageHandler.CanHandle(message))
            {
                clientMessageHandler.Handle(message);
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