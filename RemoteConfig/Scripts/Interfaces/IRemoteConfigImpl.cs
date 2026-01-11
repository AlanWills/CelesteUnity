using System;
using System.Threading.Tasks;
using Celeste.RemoteConfig.Objects;

namespace Celeste.RemoteConfig
{
    public interface IRemoteConfigImpl
    {
        Task FetchData(RemoteConfigEnvironmentIds environmentIds, bool isDebugBuild);

        void AddOnDataFetchedCallback(Action<string> dataCallback);
        void RemoveOnDataFetchedCallback(Action<string> dataCallback);
    }
}
