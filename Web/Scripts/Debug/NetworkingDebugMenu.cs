using System.Collections.Generic;
using Celeste.Debug.Menus;
using Unity.Netcode;
using UnityEngine;

namespace Celeste.Web.Debug
{
    [CreateAssetMenu(fileName = nameof(NetworkingDebugMenu), menuName = CelesteMenuItemConstants.WEB_MENU_ITEM + "Debug/Networking Debug Menu", order = CelesteMenuItemConstants.WEB_MENU_ITEM_PRIORITY)]
    public class NetworkingDebugMenu : DebugMenu
    {
        protected override void OnDrawMenu()
        {
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Host"))
                {
                    NetworkManager.Singleton.StartHost();
                }

                if (GUILayout.Button("Client"))
                {
                    NetworkManager.Singleton.StartClient();
                }

                if (GUILayout.Button("Server"))
                {
                    NetworkManager.Singleton.StartServer();
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.Label($"Connected Clients", CelesteGUIStyles.BoldLabel);

            var connectedClients = NetworkManager.Singleton.ConnectedClients;
            if (connectedClients == null || connectedClients.Count == 0)
            {
                DrawNoConnectedClientsGUI();
            }
            else
            {
                DrawConnectedClientsGUI(connectedClients);
            }
        }

        private void DrawNoConnectedClientsGUI()
        {
            GUILayout.Label($"No connected clients found on Network Manager!");
        }

        private void DrawConnectedClientsGUI(IReadOnlyDictionary<ulong, NetworkClient> connectedClients)
        {
            foreach (var connectedClient in connectedClients)
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label($"Client ID: {connectedClient.Key}");

                    if (GUILayout.Button("Ping Server"))
                    {
                        SendMessageToServerRpc("");
                    }
                }
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void SendMessageToServerRpc(string message, ServerRpcParams rpcParams = default)
        {
            ulong clientId = rpcParams.Receive.SenderClientId;
            UnityEngine.Debug.Log($"Player '{clientId}' sent '{message}' to Server.");
        }

        [ClientRpc]
        private void SendMessageToClientRpc(string message)
        {
            UnityEngine.Debug.Log($"Server sent: '{message}' to Clients.");
        }
    }
}
