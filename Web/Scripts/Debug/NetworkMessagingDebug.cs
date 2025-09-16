using Unity.Netcode;

namespace Celeste.Web.Debug
{
    public class NetworkMessagingDebug : NetworkBehaviour
    {
        public void SendMessageToServerRpc1(string message)
        {
            SendMessageToServerRpc(message);
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void SendMessageToServerRpc(string message, ServerRpcParams rpcParams = default)
        {
            ulong clientId = rpcParams.Receive.SenderClientId;
            UnityEngine.Debug.LogError($"Player '{clientId}' sent '{message}' to Server.");
        }
        
        public void SendMessageToClientsRpc1(string message)
        {
            SendMessageToClientRpc(message);
        }
        
        public void SendMessageToClientRpc1(string message, ulong clientId)
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
        public void SendMessageToClientRpc(string message, ClientRpcParams rpcParams = default)
        {
            UnityEngine.Debug.LogError($"Client {NetworkManager.Singleton.LocalClientId} received: '{message}' from Server.");
        }
    }
}