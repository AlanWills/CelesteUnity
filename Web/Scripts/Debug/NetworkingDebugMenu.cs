using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Celeste.Debug.Menus;
using Unity.Netcode;
using UnityEngine;

namespace Celeste.Web.Debug
{
    [CreateAssetMenu(fileName = nameof(NetworkingDebugMenu), menuName = CelesteMenuItemConstants.WEB_MENU_ITEM + "Debug/Networking Debug Menu", order = CelesteMenuItemConstants.WEB_MENU_ITEM_PRIORITY)]
    public class NetworkingDebugMenu : DebugMenu
    {
        #region Task And Status

        private class TaskAndStatus : IProgress<string>
        {
            public Task Task;
            public string Status;
            
            public void Report(string value)
            {
                Status = value;
            }
        }
        
        #endregion
        
        #region Properties and Fields
        
        [SerializeField] private NetworkingManagerAPI networkingManagerAPI;

        private readonly TaskAndStatus becomeHostTask = new();
        private readonly TaskAndStatus becomeServerTask = new();
        private readonly TaskAndStatus becomeClientTask = new();
        private string joinCode;
        
        #endregion

        protected override void OnDrawMenu()
        {
            GUILayout.Label($"Has Default Player Prefab? {networkingManagerAPI.HasDefaultPlayerPrefab}");
            GUILayout.Label($"Will Auto Spawn Player Prefab? {networkingManagerAPI.WillAutoSpawnPlayerPrefab}");
            
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Join Code: ");
                joinCode = GUILayout.TextField(joinCode);
            }

            DrawNetworkManagerGUI();
            
            if (networkingManagerAPI.Server.Exists)
            {
                if (networkingManagerAPI.Server.HasConnectedClients)
                {
                    DrawConnectedToClientsGUI(networkingManagerAPI.Server);
                }
                else
                {
                    DrawNotConnectedToClientsGUI();
                }
            }
            
            if (networkingManagerAPI.Client.Exists)
            {
                DrawConnectedToServerGUI(networkingManagerAPI.Client);
            }
        }

        private void DrawNetworkManagerGUI()
        {
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Host"))
                {
                    becomeHostTask.Task = networkingManagerAPI.BecomeHost(becomeHostTask);
                }
                
                GUILayout.Label(becomeHostTask.Status);
            }

            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Client"))
                {
                    becomeClientTask.Task = networkingManagerAPI.BecomeClient(becomeClientTask, joinCode);
                }
                
                GUILayout.Label(becomeClientTask.Status);
            }
        }

        private static void DrawNotConnectedToClientsGUI()
        {
            GUILayout.Label("No connected clients found on Network Manager!");
        }

        private static void DrawConnectedToClientsGUI(INetworkingServer server)
        {
            GUILayout.Label($"Server Network Object Null?: {server.HasNetworkObject}");
            GUILayout.Label("Connected Clients", CelesteGUIStyles.BoldLabel);

            foreach (var connectedClient in server.ConnectedClients)
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label($"Client ID: {connectedClient}");
                    
                    if (GUILayout.Button("Ping Client"))
                    {
                        server.SendMessageToClient($"Hello from Server to Client {connectedClient}!", connectedClient);
                    }
                }
            }
        }

        private static void DrawConnectedToServerGUI(INetworkingClient client)
        {
            GUILayout.Label($"Client Id is: {client.Id}");
            GUILayout.Label($"Client Network Object Null?: {client.HasNetworkObject}");
            
            if (GUILayout.Button("Ping Server"))
            {
                client.SendMessageToServer($"Hello from Client {client.Id}!");
            }
        }
    }
}
