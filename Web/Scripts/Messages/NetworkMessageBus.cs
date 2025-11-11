#if USE_NETCODE
using Unity.Netcode;

namespace Celeste.Web.Messages
{
    public class NetworkMessageBus : NetworkMessageHandler
    {
        [ServerRpc]
        public void RequestDisconnectServerRpc(ulong clientIdToDisconnect, ServerRpcParams rpcParams = default)
        {
            UnityEngine.Debug.Log($"Received disconnect request for Client {clientIdToDisconnect} on Server from Client {rpcParams.Receive.SenderClientId}.", CelesteLog.Web);
            Server.DisconnectClient(clientIdToDisconnect);
        }

        [ServerRpc]
        public void PingServerRpc(string message, ServerRpcParams rpcParams = default)
        {
            UnityEngine.Debug.Log($"Received message: {message} as a ping from Client {rpcParams.Receive.SenderClientId}.", CelesteLog.Web);
        }

        [ClientRpc]
        public void PingClientRpc(string message, ClientRpcParams rpcParams = default)
        {
            UnityEngine.Debug.Log($"Received message: {message} as a ping from Server.", CelesteLog.Web);
        }

        [ServerRpc]
        public void SendMessageToServerRpc(string message, ServerRpcParams rpcParams = default)
        {
#if NETWORK_MESSAGING_DEBUGGING
            UnityEngine.Debug.Log($"Message Received For Server: {message} ({name}).", CelesteLog.Web);
#endif
            UnityEngine.Debug.Assert(Server != null, $"Server is null on {name}!", CelesteLog.Web);
            Server?.OnMessageReceived(message);
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
        public void SendMessageToClientRpc(string message, ClientRpcParams rpcParams = default)
        {
#if NETWORK_MESSAGING_DEBUGGING
            UnityEngine.Debug.Log($"Message Received For Client: {message} ({name}).", CelesteLog.Web);
#endif
            UnityEngine.Debug.Assert(Client != null, $"Client is null on {name}!", CelesteLog.Web);
            Client?.GetNetworkMessageHandler<IRawMessageNetworkHandler>().OnMessageReceived(message);
        }
    }
}
#endif