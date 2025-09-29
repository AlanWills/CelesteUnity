#if USE_NETCODE
using System;
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
        
        [SerializeField] private NetworkingManagerAPI api;
        [SerializeField] private NetworkManager networkManager;
        [SerializeField] private UnityTransport unityTransport;
        [SerializeField] private NetworkingMessageSerializationFactory networkingMessageSerializationFactory;
        [SerializeField] private NetworkingMessageHandlerFactory clientNetworkingMessageHandlerFactory;
        [SerializeField] private NetworkingMessageHandlerFactory serverNetworkingMessageHandlerFactory;
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
            RemoveClientCallbacks();
            RemoveServerCallbacks();
        }

        public async Task BecomeHost(IProgress<string> progress)
        {
            SetUpServerCallbacks();
            
            Server = new DisabledNetworkingServer();
            Client = new DisabledNetworkingClient();
            
            progress?.Report("Becoming Host - Signing In");
            await InitializeAndLogInUnityServices();
            
            progress?.Report("Becoming Host - Create Allocation");
            var allocation = await RelayService.Instance.CreateAllocationAsync(MAX_CONNECTIONS);
            
            progress?.Report("Becoming Host - Getting Join Code");
            unityTransport.SetRelayServerData(allocation.ToRelayServerData(CONNECTION_TYPE));
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            
            Server = new ActiveNetworkingServer(
                joinCode,
                networkingMessageSerializationFactory,
                networkingMessageSerializationFactory,
                serverNetworkingMessageHandlerFactory);

            if (networkManager.StartHost())
            {
                progress?.Report("Becoming Host - Complete");
            }
            else
            {
                Server = new DisabledNetworkingServer();
                progress?.Report("Becoming Host - Failed");
            }
        }

        public async Task BecomeClient(IProgress<string> progress, string joinCode)
        {
            SetUpClientCallbacks();
            
            Server = new DisabledNetworkingServer();
            Client = new DisabledNetworkingClient();
            
            progress?.Report("Becoming Client - Signing In");
            await InitializeAndLogInUnityServices();

            progress?.Report($"Becoming Client - Joining With Code {joinCode}");
            var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            
            unityTransport.SetRelayServerData(allocation.ToRelayServerData(CONNECTION_TYPE));
            
            if (!string.IsNullOrEmpty(joinCode) && networkManager.StartClient())
            {
                progress?.Report("Becoming Client - Complete");
            }
            else
            {
                progress?.Report("Becoming Client - Failed");
            }
        }

        private void SetUpServerCallbacks()
        {
            RemoveServerCallbacks();

            if (networkManager.NetworkConfig.ConnectionApproval)
            {
                networkManager.ConnectionApprovalCallback += OnConnectionApproval;
            }

            networkManager.OnClientConnectedCallback += OnClientConnectedToServer;
            networkManager.OnClientDisconnectCallback += OnClientDisconnectedFromServer;
        }

        private void RemoveServerCallbacks()
        {
            if (networkManager.NetworkConfig.ConnectionApproval)
            {
                networkManager.ConnectionApprovalCallback -= OnConnectionApproval;
            }

            networkManager.OnClientConnectedCallback -= OnClientConnectedToServer;
            networkManager.OnClientDisconnectCallback += OnClientDisconnectedFromServer;
        }

        private void SetUpClientCallbacks()
        {
            RemoveClientCallbacks();
            
            networkManager.OnClientConnectedCallback += OnLocalClientConnected;
        }

        private void RemoveClientCallbacks()
        {
            networkManager.OnClientConnectedCallback -= OnLocalClientConnected;
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
            UnityEngine.Debug.Assert(response.CreatePlayerObject, "Create Player Object was false!!!", CelesteLog.Web);
            response.CreatePlayerObject = true;
        }
        
        private void OnClientConnectedToServer(ulong clientId)
        { 
            UnityEngine.Debug.Log($"Client {clientId} connected!", CelesteLog.Web);

            // If the client already has a player, do nothing
            if (networkManager.ConnectedClients[clientId].PlayerObject != null)
            {
                // Need to tweak this to be the same scene as our eventual network message bus/manager thing
                SceneManager.MoveGameObjectToScene(networkManager.ConnectedClients[clientId].PlayerObject.gameObject, networkManager.gameObject.scene);
                UnityEngine.Debug.Log($"PlayerObject was already spawned for {clientId}: {networkManager.ConnectedClients[clientId].PlayerObject.name}", CelesteLog.Web);
            }
            else
            {
                UnityEngine.Debug.LogError($"No PlayerObject spawned for {clientId}!");

                // Instantiate the prefab on the server
                // Need to tweak this to be the same scene as our eventual network message bus/manager thing
                GameObject playerInstance = Instantiate(networkManager.NetworkConfig.PlayerPrefab, networkManager.gameObject.scene) as GameObject;
                NetworkObject netObj = playerInstance.GetComponent<NetworkObject>();
                UnityEngine.Debug.Assert(netObj != null, $"Default player prefab must have a {nameof(NetworkObject)} component!", CelesteLog.Web);
                
                // Spawn with ownership assigned to the client
                netObj.SpawnAsPlayerObject(clientId, true);
                UnityEngine.Debug.LogError($"Spawned player object for client {clientId}", CelesteLog.Web);
            }

            networkManager.ConnectedClients[clientId].PlayerObject.name = $"NetworkMessaging - Client Id {clientId}";
            
            NetworkObject networkObject = networkManager.ConnectedClients[clientId].PlayerObject;
            UnityEngine.Debug.Assert(networkObject != null, "Client Started, but Player Object is null!", CelesteLog.Web);
            NetworkMessaging networkMessaging = networkObject.GetComponent<NetworkMessaging>();
            UnityEngine.Debug.Assert(networkMessaging != null, $"Client Started, but {nameof(NetworkMessaging)} is null!", CelesteLog.Web);

            ActiveNetworkingClient client = new ActiveNetworkingClient(
                clientId,
                networkMessaging,
                networkingMessageSerializationFactory,
                networkingMessageSerializationFactory,
                clientNetworkingMessageHandlerFactory);
            Server.AddConnectedClient(client);
        }

        private void OnLocalClientConnected(ulong clientId)
        {
            if (networkManager.LocalClientId == clientId)
            {
                NetworkObject networkObject = networkManager.LocalClient.PlayerObject;
                UnityEngine.Debug.Assert(networkObject != null, "Client Started, but Player Object is null!", CelesteLog.Web);
                NetworkMessaging networkMessaging = networkObject.GetComponent<NetworkMessaging>();
                UnityEngine.Debug.Assert(networkMessaging != null, $"Client Started, but {nameof(NetworkMessaging)} is null!", CelesteLog.Web);

                Client = new ActiveNetworkingClient(
                    clientId,
                    networkMessaging,
                    networkingMessageSerializationFactory,
                    networkingMessageSerializationFactory,
                    clientNetworkingMessageHandlerFactory);
                
                networkManager.ConnectedClients[clientId].PlayerObject.name = $"NetworkMessaging - Client Id {clientId}";
                SceneManager.MoveGameObjectToScene(networkObject.gameObject, networkManager.gameObject.scene);
            }
        }

        private void OnClientDisconnectedFromServer(ulong clientId)
        {
            Server.RemoveConnectedClient(clientId);
        }
        
        #endregion
    }
}
#endif