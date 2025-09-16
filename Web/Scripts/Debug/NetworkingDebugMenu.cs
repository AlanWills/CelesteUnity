using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Celeste.Debug.Menus;
using Celeste.Events;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celeste.Web.Debug
{
    [CreateAssetMenu(fileName = nameof(NetworkingDebugMenu), menuName = CelesteMenuItemConstants.WEB_MENU_ITEM + "Debug/Networking Debug Menu", order = CelesteMenuItemConstants.WEB_MENU_ITEM_PRIORITY)]
    public class NetworkingDebugMenu : DebugMenu
    {
        #region Properties and Fields
        
        [SerializeField] private StringEvent pingClients;
        [SerializeField] private StringEvent pingServer;

        private string joinCode;
        
        #endregion

        protected override void OnShowMenu()
        {
            base.OnShowMenu();
            
            NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
            {
                // Only the server should spawn the player
                if (!NetworkManager.Singleton.IsServer)
                {
                    return;
                }
                
                UnityEngine.Debug.LogError($"Client {clientId} connected!");

                // If the client already has a player, do nothing
                if (NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject != null)
                {
                    // Need to tweak this to be the same scene as our eventual network message bus/manager thing
                    SceneManager.MoveGameObjectToScene(NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject, NetworkManager.Singleton.gameObject.scene);
                    UnityEngine.Debug.LogError($"Spawned PlayerObject for {clientId}: {NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.name}");
                    return;
                }
                
                UnityEngine.Debug.LogError($"No PlayerObject spawned for {clientId}!");

                // Instantiate the prefab on the server
                // Need to tweak this to be the same scene as our eventual network message bus/manager thing
                GameObject playerInstance = Instantiate(NetworkManager.Singleton.NetworkConfig.PlayerPrefab, NetworkManager.Singleton.gameObject.scene) as GameObject;
                NetworkObject netObj = playerInstance.GetComponent<NetworkObject>();

                if (netObj == null)
                {
                    UnityEngine.Debug.LogError("Default player prefab must have a NetworkObject component!");
                    Destroy(playerInstance);
                    return;
                }

                // Spawn with ownership assigned to the client
                netObj.SpawnAsPlayerObject(clientId, true);
                UnityEngine.Debug.LogError($"Spawned player object for client {clientId}");
            };
            
            NetworkManager.Singleton.ConnectionApprovalCallback += (request, response) =>
            {
                response.Approved = true;

                // If this is false, Netcode skips the Default Player Prefab auto-spawn:
                UnityEngine.Debug.Assert(response.CreatePlayerObject, "Create Player Object was false!!!");
                response.CreatePlayerObject = true;
            };
        }

        protected override void OnDrawMenu()
        {
            if (NetworkManager.Singleton == null)
            {
                GUILayout.Label($"{nameof(NetworkManager)} is not created yet...");
                return;
            }

            GUILayout.Label($"Default Player Prefab Null? {NetworkManager.Singleton.NetworkConfig.PlayerPrefab == null}");
            GUILayout.Label($"Auto Spawn Player Prefab? {NetworkManager.Singleton.AutoSpawnPlayerPrefabClientSide}");
            
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Join Code: ");
                joinCode = GUILayout.TextField(joinCode);
            }

            DrawNetworkManagerGUI();
            
            if (NetworkManager.Singleton.IsServer)
            {
                var connectedClients = NetworkManager.Singleton.ConnectedClients;
                if (connectedClients == null || connectedClients.Count == 0)
                {
                    DrawNotConnectedToClientsGUI();
                }
                else
                {
                    DrawConnectedToClientsGUI(connectedClients);
                }
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                if (NetworkManager.Singleton.IsConnectedClient)
                {
                    DrawConnectedToServerGUI();
                }
                else
                {
                    DrawNotConnectedToServerGUI();
                }
            }
        }

        private void DrawNetworkManagerGUI()
        {
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Host"))
                {
                    StartHostWithRelay(4, "dtls");
                }

                if (GUILayout.Button("Client"))
                {
                    StartClientWithRelay(joinCode, "dtls");
                }
            }
        }
        
        private async Task StartHostWithRelay(int maxConnections, string connectionType)
        {
            await UnityServices.InitializeAsync();
            
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            var allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(allocation.ToRelayServerData(connectionType));
            var thisJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            joinCode = NetworkManager.Singleton.StartHost() ? thisJoinCode : null;
        }
        
        private static async Task StartClientWithRelay(string serverJoinCode, string connectionType)
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode: serverJoinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(allocation.ToRelayServerData(connectionType));
            bool joinSuccess = !string.IsNullOrEmpty(serverJoinCode) && NetworkManager.Singleton.StartClient();

            if (joinSuccess)
            {
                UnityEngine.Debug.Log("Successfully joined!");
            }
            else
            {
                UnityEngine.Debug.LogError("Failed to join...");
            }
        }

        private void DrawNotConnectedToClientsGUI()
        {
            GUILayout.Label("No connected clients found on Network Manager!");
        }

        private void DrawConnectedToClientsGUI(IReadOnlyDictionary<ulong, NetworkClient> connectedClients)
        {
            GUILayout.Label("Connected Clients", CelesteGUIStyles.BoldLabel);

            foreach (var connectedClient in connectedClients)
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label($"Client ID: {connectedClient.Key}");
                    
                    if (GUILayout.Button("Ping"))
                    {
                        connectedClient.Value.PlayerObject.GetComponent<NetworkMessagingDebug>().SendMessageToClientRpc1("Hello from Server!", connectedClient.Key);
                    }
                }
            }
        }

        private void DrawNotConnectedToServerGUI()
        {
            GUILayout.Label("No connected Server found on Network Manager!");
        }

        private void DrawConnectedToServerGUI()
        {
            GUILayout.Label($"Client Id is: {NetworkManager.Singleton.LocalClientId}");
            GUILayout.Label($"Client Network Object Null?: {NetworkManager.Singleton.LocalClient.PlayerObject == null}");
            
            if (GUILayout.Button("Ping Server"))
            {
                NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<NetworkMessagingDebug>().SendMessageToServerRpc($"Hello from Client {NetworkManager.Singleton.LocalClientId}!");
            }
        }
    }
}
