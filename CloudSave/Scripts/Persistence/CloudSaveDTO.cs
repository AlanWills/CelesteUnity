using System;

namespace Celeste.CloudSave.Persistence
{
    [Serializable]
    public class CloudSaveDTO
    {
        public DateTimeOffset playtimeFirstStart;
        public int implementation;
    }
}
