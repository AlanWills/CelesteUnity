using System.Collections.Generic;
using Unity.Netcode;

namespace Celeste.Web.Messages
{
    public class NetworkMessaging : NetworkBehaviour
    {
        public void SendMessageToServer(string message)
        {
            SendMessageToServerRpc(message);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SendMessageToServerRpc(string message, ServerRpcParams rpcParams = default)
        {
            ulong clientId = rpcParams.Receive.SenderClientId;
            UnityEngine.Debug.Log($"Player '{clientId}' sent '{message}' to Server.");
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
            UnityEngine.Debug.Log($"Client {NetworkManager.Singleton.LocalClientId} received: '{message}' from Server.");
        }
    }
}