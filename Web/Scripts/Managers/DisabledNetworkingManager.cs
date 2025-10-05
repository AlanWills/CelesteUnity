using System;
using System.Threading.Tasks;
using Celeste.Web.Objects;
using Unity.Netcode;
using UnityEngine;

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
            progress?.Report("Networking Disabled, ignoring become Host request..");
            return Task.CompletedTask;
        }

        public Task BecomeServer(IProgress<string> progress)
        {
            progress?.Report("Networking Disabled, ignoring become Server request..");
            return Task.CompletedTask;
        }

        public Task BecomeClient(IProgress<string> progress, string joinCode)
        {
            progress?.Report("Networking Disabled, ignoring become Client request.");
            return Task.CompletedTask;
        }

        public NetworkObject Spawn(IProgress<string> progress, NetworkObject networkObject)
        {
            progress?.Report("Networking Disabled, ignoring spawn request.");
            return null;
        }
    }
}