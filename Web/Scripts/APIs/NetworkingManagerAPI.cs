using System;
using System.Threading.Tasks;
#if USE_NETCODE
using Celeste.Web.Managers;
using Unity.Netcode;
#endif
using UnityEngine;

namespace Celeste.Web
{
    [CreateAssetMenu(fileName = nameof(NetworkingManagerAPI), menuName = CelesteMenuItemConstants.WEB_MENU_ITEM + "API/Networking Manager", order = CelesteMenuItemConstants.WEB_MENU_ITEM_PRIORITY)]
    public class NetworkingManagerAPI : ScriptableObject
#if USE_NETCODE
        , INetworkingManager
#endif
    {
#if USE_NETCODE
        #region Properties and Fields
        
        public bool HasDefaultPlayerPrefab => impl.HasDefaultPlayerPrefab;
        public bool WillAutoSpawnPlayerPrefab => impl.WillAutoSpawnPlayerPrefab;
        public INetworkingServer Server => impl.Server;
        public INetworkingClient Client => impl.Client;

        private INetworkingManager impl = new DisabledNetworkingManager();
        
        #endregion

        public void Initialize(INetworkingManager managerImpl)
        {
            Shutdown();
            impl = managerImpl;
            Setup();
        }
        
        public void Setup()
        {
            impl.Setup();
        }

        public void Shutdown()
        {
            impl?.Shutdown();
            impl = new DisabledNetworkingManager();
        }
        
        public Task BecomeHost(IProgress<string> progress)
        {
            return impl.BecomeHost(progress);
        }
        
        public Task BecomeClient(IProgress<string> progress, string joinCode)
        {
            return impl.BecomeClient(progress, joinCode);
        }

        public NetworkObject Spawn(IProgress<string> progress, NetworkObject networkObject)
        {
            return impl.Spawn(progress, networkObject);
        }
#endif
    }
}