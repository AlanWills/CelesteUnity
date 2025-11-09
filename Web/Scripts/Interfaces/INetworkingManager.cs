#if USE_NETCODE
using System;
using System.Threading.Tasks;
using Unity.Netcode;

namespace Celeste.Web
{
    public interface INetworkingManager
    {
        bool HasDefaultPlayerPrefab { get; }
        bool WillAutoSpawnPlayerPrefab { get; }
        INetworkingServer Server { get; }
        INetworkingClient Client { get; }

        void Setup();
        void Shutdown();
        
        Task BecomeHost(IProgress<string> progress);
        Task BecomeClient(IProgress<string> progress, string joinCode);

        NetworkObject Spawn(IProgress<string> progress, NetworkObject networkObject);
    }
}
#endif
