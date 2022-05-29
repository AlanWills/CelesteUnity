using Celeste.Persistence;
using UnityEngine;

namespace Celeste.LiveOps.Persistence
{
    [AddComponentMenu("Celeste/Live Ops/Live Ops Manager")]
    public class LiveOpsManager : PersistentSceneManager<LiveOpsManager, LiveOpsDTO>
    {
        #region Properties and Fields

        public const string FILE_NAME = "LiveOps.dat";
        protected override string FileName => FILE_NAME;

        // This is fallback data for the client to use, in case we aren't able to obtain anything from the game server
        [SerializeField] private LiveOpsSchedule liveOpsSchedule;
        [SerializeField] private LiveOpsRecord liveOpsRecord;

        #endregion

        #region Save/Load

        protected override LiveOpsDTO Serialize()
        {
            return new LiveOpsDTO(liveOpsRecord);
        }

        protected override void Deserialize(LiveOpsDTO dto)
        {
            // Add all our live ops from the save data
            foreach (LiveOpDTO liveOpDTO in dto.liveOps)
            {
                liveOpsRecord.AddLiveOp(liveOpDTO);
            }

            // Add any missing liveops from our schedule too
            foreach (LiveOpDTO liveOpDTO in liveOpsSchedule.Items)
            {
                liveOpsRecord.AddLiveOp(liveOpDTO);
            }
        }

        protected override void SetDefaultValues()
        {
            // Initialize our record with the live ops in the schedule
            foreach (LiveOpDTO liveOpDTO in liveOpsSchedule.Items)
            {
                liveOpsRecord.AddLiveOp(liveOpDTO);
            }
        }

        #endregion
    }
}
