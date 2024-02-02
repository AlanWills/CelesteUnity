using System;

namespace Celeste.RemoteConfig.Persistence
{
    [Serializable]
    public class RemoteConfigManagerDTO
    {
        public int dataSource;
        public string cachedConfig;

        public RemoteConfigManagerDTO(RemoteConfigRecord remoteConfigRecord)
        {
            dataSource = (int)remoteConfigRecord.DataSource;
            cachedConfig = remoteConfigRecord.ToJson();
        }
    }
}
