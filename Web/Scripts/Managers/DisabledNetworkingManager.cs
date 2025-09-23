using System;
using System.Threading.Tasks;
using Celeste.Web.Objects;

namespace Celeste.Web.Managers
{
    public class DisabledNetworkingManager : INetworkingManager
    {
        #region Properties and Fields
        
        public bool HasDefaultPlayerPrefab => false;
        public bool WillAutoSpawnPlayerPrefab => false;
        public INetworkingServer Server { get; } = new DisabledNetworkingServer();
        public INetworkingClient Client { get; } = new DisabledNetworkingClient();

        #endregion
        
        public void Setup() { }

        public void Shutdown() { }
        
        public Task BecomeHost(IProgress<string> progress)
        {
            progress?.Report("Networking Disabled.");
            return Task.CompletedTask;
        }

        public Task BecomeServer(IProgress<string> progress)
        {
            progress?.Report("Networking Disabled.");
            return Task.CompletedTask;
        }

        public Task BecomeClient(IProgress<string> progress, string joinCode)
        {
            progress?.Report("Networking Disabled.");
            return Task.CompletedTask;
        }
    }
}