using System;
using System.Threading.Tasks;
using Celeste.RemoteConfig.Objects;

namespace Celeste.RemoteConfig
{
    public class DisabledRemoteConfigImpl : IRemoteConfigImpl
    {
        public Task FetchData(RemoteConfigEnvironmentIds environmentIds, bool isDebugBuild)
        {
            return Task.CompletedTask;
        }

        public void AddOnDataFetchedCallback(Action<string> dataCallback) { }

        public void RemoveOnDataFetchedCallback(Action<string> dataCallback) { }
    }
}
