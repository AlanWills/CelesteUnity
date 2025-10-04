#if USE_NETCODE
using System.Collections.Generic;
using Celeste.Web.Messages;

namespace Celeste.Web.Objects
{
    public class ActiveNetworkingClient : INetworkingClient
    {
        #region Properties and Fields
        
        public ulong Id { get; }
        public bool Exists => true;
        public bool HasNetworkObject => networkMessageBus != null;
        public IReadOnlyList<INetworkMessageHandler> NetworkMessageHandlers => networkMessageHandlers;
        
        private readonly INetworkingMessageSerializer serializer;
        private readonly INetworkingMessageDeserializer deserializer;
        private readonly INetworkingMessageHandler clientMessageHandler;
        private readonly NetworkMessageBus networkMessageBus;
        private readonly List<INetworkMessageHandler> networkMessageHandlers = new();

        #endregion

        public ActiveNetworkingClient(
            ulong clientId,
            NetworkMessageBus networkMessageBus,
            INetworkingMessageSerializer serializer,
            INetworkingMessageDeserializer deserializer,
            INetworkingMessageHandler clientMessageHandler)
        {
            Id = clientId;
            this.networkMessageBus = networkMessageBus;
            this.serializer = serializer;
            this.deserializer = deserializer;
            this.clientMessageHandler = clientMessageHandler;
            
            networkMessageHandlers.AddRange(networkMessageBus.GetComponentsInChildren<INetworkMessageHandler>());
            foreach (INetworkMessageHandler clientMessaging in networkMessageHandlers)
            {
                clientMessaging.SetClient(this);
            }
        }

        public void Ping(string message)
        {
            UnityEngine.Debug.Assert(networkMessageBus != null, $"Attempting to send a message without {nameof(NetworkMessageBus)} being set!", CelesteLog.Web);
            networkMessageBus?.PingClientRpc(message);
        }

        public void PingServer(string message)
        {
            UnityEngine.Debug.Assert(networkMessageBus != null, $"Attempting to send a message without {nameof(NetworkMessageBus)} being set!", CelesteLog.Web);
            networkMessageBus?.PingServerRpc(message);
        }

        public void SendMessage(string messageAsString)
        {
            UnityEngine.Debug.Assert(networkMessageBus != null, $"Attempting to send a message without {nameof(NetworkMessageBus)} being set!", CelesteLog.Web);
            networkMessageBus?.SendMessageToClient(messageAsString, Id);
        }
        
        public void SendMessageToServer<T>(NetworkingMessage<T> message)
        {
            string messageAsString = Serialize(message);
            UnityEngine.Debug.Assert(networkMessageBus != null, $"Attempting to send a message without {nameof(NetworkMessageBus)} being set!", CelesteLog.Web);
            networkMessageBus?.SendMessageToServerRpc(messageAsString);
        }

        public T GetNetworkMessageHandler<T>() where T : INetworkMessageHandler
        {
            foreach (INetworkMessageHandler networkMessageHandler in networkMessageHandlers)
            {
                if (networkMessageHandler is T foundHandler)
                {
                    return foundHandler;
                }
            }

            return default;
        }

        private string Serialize<T>(NetworkingMessage<T> message)
        {
            return serializer.Serialize(message);
        }
        
        public void OnNetworkingMessageReceived(string rawMessage)
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
    }
}
#endif