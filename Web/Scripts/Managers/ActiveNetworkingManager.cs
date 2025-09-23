using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Celeste.Parameters;
using Celeste.Web.Messages;
using Celeste.Web.Objects;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celeste.Web.Managers
{
    public class ActiveNetworkingManager : MonoBehaviour, INetworkingManager
    {
        #region Properties and Fields

        public bool HasDefaultPlayerPrefab => networkManager.NetworkConfig.PlayerPrefab != null;
        public bool WillAutoSpawnPlayerPrefab => networkManager.NetworkConfig.AutoSpawnPlayerPrefabClientSide;
        public INetworkingServer Server { get; private set; } = new DisabledNetworkingServer();
        public INetworkingClient Client { get; private set; } = new DisabledNetworkingClient();
        public bool HasConnectedClients => networkManager.ConnectedClients?.Count > 0;
        public IEnumerable<ulong> ConnectedClients => networkManager.ConnectedClients.Keys;
        
        [SerializeField] private NetworkingManagerAPI api;
        [SerializeField] private NetworkManager networkManager;
        [SerializeField] private UnityTransport unityTransport;
        [SerializeField] private NetworkingMessageSerializer networkingMessageSerializer;
        [SerializeField] private NetworkingMessageDeserializer networkingMessageDeserializer;
        [SerializeField] private BoolValue isDebugBuild;
        [SerializeField] private LogLevel debugLogLevel =  LogLevel.Developer;
        [SerializeField] private LogLevel prodLogLevel = LogLevel.Error;
        
        private const int MAX_CONNECTIONS = 4;
        private const string CONNECTION_TYPE = "dtls";
        
        #endregion
        
        #region Unity Methods

        private void OnEnable()
        {
            networkManager.LogLevel = isDebugBuild ? debugLogLevel : prodLogLevel;
            
            api.Initialize(this);
        }

        private void OnDisable()
        {
            api.Shutdown();
        }

        #endregion
        
        public void Setup()
        {
        }

        public void Shutdown()
        {
            RemoveServerCallbacks();
        }

        public async Task BecomeHost(IProgress<string> progress)
        {
            RemoveServerCallbacks();
            
            progress?.Report("Becoming Host - Signing In");
            await InitializeAndLogInUnityServices();
            
            progress?.Report("Becoming Host - Create Allocation");
            var allocation = await RelayService.Instance.CreateAllocationAsync(MAX_CONNECTIONS);
            
            progress?.Report("Becoming Host - Getting Join Code");
            unityTransport.SetRelayServerData(allocation.ToRelayServerData(CONNECTION_TYPE));
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            
            if (networkManager.StartHost())
            {
                NetworkObject networkObject = networkManager.LocalClient.PlayerObject;
                NetworkMessaging networkMessaging = networkObject.GetComponent<NetworkMessaging>();

                Server = new ActiveNetworkingServer(
                    joinCode, 
                    networkObject,
                    networkMessaging,
                    networkingMessageSerializer,
                    networkingMessageDeserializer);
                Client = new ActiveNetworkingClient(
                    networkManager.LocalClientId, 
                    networkObject, 
                    networkMessaging,
                    networkingMessageSerializer,
                    networkingMessageDeserializer);
                
                // Add this manually here because we won't have the callback triggered for the Host's client object
                Server.AddConnectedClient(networkManager.LocalClientId);
                progress?.Report("Becoming Host - Complete");
            }
            else
            {
                Server = new DisabledNetworkingServer();
                Client = new DisabledNetworkingClient();
                progress?.Report("Becoming Host - Failed");
            }
            
            SetUpServerCallbacks();
        }

        public async Task BecomeClient(IProgress<string> progress, string joinCode)
        {
            progress?.Report("Becoming Client - Signing In");
            await InitializeAndLogInUnityServices();

            progress?.Report($"Becoming Client - Joining With Code {joinCode}");
            var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            
            unityTransport.SetRelayServerData(allocation.ToRelayServerData(CONNECTION_TYPE));
            
            if (!string.IsNullOrEmpty(joinCode) && networkManager.StartClient())
            {
                Server = new DisabledNetworkingServer();
                Client = new ActiveNetworkingClient(
                    networkManager.LocalClientId, 
                    networkManager.LocalClient.PlayerObject,
                    networkManager.LocalClient.PlayerObject.GetComponent<NetworkMessaging>(),
                    networkingMessageSerializer,
                    networkingMessageDeserializer);
                progress?.Report("Becoming Client - Complete");
            }
            else
            {
                Server = new DisabledNetworkingServer();
                Client = new DisabledNetworkingClient();
                progress?.Report("Becoming Client - Failed");
            }
        }

        private void SetUpServerCallbacks()
        {
            networkManager.ConnectionApprovalCallback += OnConnectionApproval;
            networkManager.OnClientConnectedCallback += OnClientConnected;
            networkManager.OnClientDisconnectCallback += OnClientDisconnected;
        }

        private void RemoveServerCallbacks()
        {
            networkManager.ConnectionApprovalCallback -= OnConnectionApproval;
            networkManager.OnClientConnectedCallback -= OnClientConnected;
            networkManager.OnClientDisconnectCallback += OnClientDisconnected;
        }

        private async Task InitializeAndLogInUnityServices()
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                await UnityServices.InitializeAsync();
            }

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
        
        #region Callbacks

        private void OnConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            response.Approved = true;

            // If this is false, Netcode skips the Default Player Prefab auto-spawn:
            UnityEngine.Debug.Assert(response.CreatePlayerObject, "Create Player Object was false!!!");
            response.CreatePlayerObject = true;
        }
        
        private void OnClientConnected(ulong clientId)
        {
            if (!Server.Exists)
            {
                return;
            }
                
            UnityEngine.Debug.Log($"Client {clientId} connected!");

            // If the client already has a player, do nothing
            if (networkManager.ConnectedClients[clientId].PlayerObject != null)
            {
                // Need to tweak this to be the same scene as our eventual network message bus/manager thing
                SceneManager.MoveGameObjectToScene(networkManager.ConnectedClients[clientId].PlayerObject.gameObject, networkManager.gameObject.scene);
                UnityEngine.Debug.Log($"PlayerObject was already spawned for {clientId}: {networkManager.ConnectedClients[clientId].PlayerObject.name}");
            }
            else
            {
                UnityEngine.Debug.LogError($"No PlayerObject spawned for {clientId}!");

                // Instantiate the prefab on the server
                // Need to tweak this to be the same scene as our eventual network message bus/manager thing
                GameObject playerInstance = Instantiate(networkManager.NetworkConfig.PlayerPrefab, networkManager.gameObject.scene) as GameObject;
                NetworkObject netObj = playerInstance.GetComponent<NetworkObject>();
                UnityEngine.Debug.Assert(netObj != null, $"Default player prefab must have a {nameof(NetworkObject)} component!");
                
                // Spawn with ownership assigned to the client
                netObj.SpawnAsPlayerObject(clientId, true);
                UnityEngine.Debug.LogError($"Spawned player object for client {clientId}");
            }
            
            Server.AddConnectedClient(clientId);
        }

        private void OnClientDisconnected(ulong clientId)
        {
            if (!Server.Exists)
            {
                return;
            }
            
            Server.RemoveConnectedClient(clientId);
        }
        
        #endregion
    }
}