using Celeste.Persistence;
using System;
using System.Collections.Generic;

namespace Celeste.LiveOps.Persistence
{
    [Serializable]
    public class LiveOpsDTO : VersionedDTO
    {
        public List<LiveOpDTO> liveOps = new List<LiveOpDTO>();

        public LiveOpsDTO(LiveOpsRecord record)
        {
            liveOps.Capacity = record.NumLiveOps;

            for (int i = 0, n = record.NumLiveOps; i < n; i++)
            {
                liveOps.Add(new LiveOpDTO(record.GetLiveOp(i)));
            }
        }
    }
}
