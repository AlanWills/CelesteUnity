using System;
using System.Threading.Tasks;

namespace Celeste.RemoteConfig
{
    public class DisabledRemoteConfigImpl : IRemoteConfigImpl
    {
        public Task FetchData(string environmentId)
        {
            return Task.CompletedTask;
        }

        public void AddOnDataFetchedCallback(Action<string> dataCallback) { }

        public void RemoveOnDataFetchedCallback(Action<string> dataCallback) { }
    }
}
