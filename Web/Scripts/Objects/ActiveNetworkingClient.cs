#if USE_NETCODE
using System.Collections.Generic;
using Celeste.Web.Managers;
using Celeste.Web.Messages;
using Unity.Netcode;

namespace Celeste.Web.Objects
{
    public class ActiveNetworkingClient : INetworkingClient
    {
        #region Properties and Fields
        
        public ulong Id { get; }
        public bool Exists => true;
        public bool HasNetworkObject => networkObject != null;
        public IReadOnlyList<INetworkMessageHandler> NetworkMessageHandlers => networkMessageHandlers;

        private readonly ActiveNetworkingManager networkingManager;
        private readonly NetworkObject networkObject;
        private readonly NetworkMessageBus networkMessageBus;
        private readonly List<INetworkMessageHandler> networkMessageHandlers = new();

        #endregion

        public ActiveNetworkingClient(ActiveNetworkingManager networkingManager, ulong clientId, NetworkObject networkObject)
        {
            Id = clientId;
            this.networkingManager = networkingManager;
            this.networkObject = networkObject;
            
            networkMessageHandlers.AddRange(networkObject.GetComponentsInChildren<INetworkMessageHandler>());
            foreach (INetworkMessageHandler clientMessaging in networkMessageHandlers)
            {
                clientMessaging.SetClient(this);
            }

            networkMessageBus = GetNetworkMessageHandler<NetworkMessageBus>();
            UnityEngine.Debug.Assert(networkMessageBus != null, $"No {nameof(NetworkMessageBus)} added to Client {Id}'s Network Object!", CelesteLog.Web);
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
        
        public void Disconnect()
        {
            networkingManager.DisconnectLocalClient();
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
    }
}
#endif