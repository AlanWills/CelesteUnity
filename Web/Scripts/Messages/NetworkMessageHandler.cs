using System;
using Unity.Netcode;

namespace Celeste.Web.Messages
{
    public class NetworkMessageHandler : NetworkBehaviour, INetworkMessageHandler
    {
        #region Properties and Fields

        protected INetworkingClient Client
        {
            get
            {
                UnityEngine.Debug.Assert(client != null, $"Client is null on {name}!", CelesteLog.Web);
                return client;
            }
        }

        protected INetworkingServer Server
        {
            get
            {
                UnityEngine.Debug.Assert(server != null, $"Server is null on {name}!", CelesteLog.Web);
                return server;
            }
        }

        [NonSerialized] private INetworkingClient client;
        [NonSerialized] private INetworkingServer server;
        
        #endregion
        
        public void SetClient(INetworkingClient networkingClient)
        { 
            client = networkingClient;
        }

        public void SetServer(INetworkingServer networkingServer)
        {
            server = networkingServer;
        }
    }
}