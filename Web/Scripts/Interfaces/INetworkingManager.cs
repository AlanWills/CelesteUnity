using System;
using System.Threading.Tasks;

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
    }
}