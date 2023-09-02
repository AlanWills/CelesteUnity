using System;
using System.Threading.Tasks;

namespace Celeste.RemoteConfig
{
    public interface IRemoteConfigImpl
    {
        Task FetchData(string environmentId);

        void AddOnDataFetchedCallback(Action<string> dataCallback);
        void RemoveOnDataFetchedCallback(Action<string> dataCallback);
    }
}
