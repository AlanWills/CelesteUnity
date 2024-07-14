using Celeste.Persistence;
using System;

namespace Celeste.CloudSave.Persistence
{
    [Serializable]
    public class CloudSaveDTO : VersionedDTO
    {
        public DateTimeOffset playtimeFirstStart;
        public int implementation;
    }
}
