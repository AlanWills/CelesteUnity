#if USE_NETCODE
using System.Collections.Generic;
using Unity.Netcode;

namespace Celeste.Web.Messages
{
    public class NetworkMessaging : NetworkBehaviour
    {
        #region Properties and Fields
        
        private INetworkingMessageReceiver clientMessageReceiver;
        private INetworkingMessageReceiver serverMessageReceiver;
        
        #endregion

        public void SetClientMessageReceiver(INetworkingMessageReceiver messageReceiver)
        {
            clientMessageReceiver = messageReceiver;
        }

        public void SetServerMessageReceiver(INetworkingMessageReceiver messageReceiver)
        {
            serverMessageReceiver = messageReceiver;
        }

        public void SendMessageToServer(string message)
        {
            SendMessageToServerRpc(message);
        }

        [ServerRpc]
        private void SendMessageToServerRpc(string message, ServerRpcParams rpcParams = default)
        {
#if NETWORK_MESSAGING_DEBUGGING
            UnityEngine.Debug.Log($"Message Received For Server: {message} ({name}).", CelesteLog.Web);
#endif
            UnityEngine.Debug.Assert(serverMessageReceiver != null, $"Server Message Receiver is null on {name}!");
            serverMessageReceiver?.OnNetworkingMessageReceived(message);
        }
        
        public void SendMessageToAllClients(string message)
        {
            SendMessageToClientRpc(message);
        }
        
        public void SendMessageToClients(string message, IReadOnlyList<ulong> clientIds)
        {
            ClientRpcParams rpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = clientIds
                }
            };
            SendMessageToClientRpc(message, rpcParams);
        }
        
        public void SendMessageToClient(string message, ulong clientId)
        {
            ClientRpcParams rpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new[] { clientId }
                }
            };
            SendMessageToClientRpc(message, rpcParams);
        }
        
        [ClientRpc]
        private void SendMessageToClientRpc(string message, ClientRpcParams rpcParams = default)
        {
#if NETWORK_MESSAGING_DEBUGGING
            UnityEngine.Debug.Log($"Message Received For Client: {message} ({name}).", CelesteLog.Web);
#endif
            UnityEngine.Debug.Assert(clientMessageReceiver != null, $"Client Message Receiver is null on {name}!");
            clientMessageReceiver?.OnNetworkingMessageReceived(message);
        }
    }
}
#endif