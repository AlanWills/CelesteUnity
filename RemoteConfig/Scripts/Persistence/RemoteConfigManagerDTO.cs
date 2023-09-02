using System;

namespace Celeste.RemoteConfig.Persistence
{
    [Serializable]
    public class RemoteConfigManagerDTO
    {
        public string cachedConfig;

        public RemoteConfigManagerDTO(RemoteConfigRecord remoteConfigRecord)
        {
            cachedConfig = remoteConfigRecord.ToJson();
        }
    }
}
