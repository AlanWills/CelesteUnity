using Celeste.Core;
using Celeste.Persistence;
using System.Collections;
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
            StartCoroutine(AddAllLiveOps(dto));
        }

        protected override void SetDefaultValues()
        {
            StartCoroutine(AddLiveOpsFromSchedule());
        }

        #endregion

        private IEnumerator AddAllLiveOps(LiveOpsDTO liveOpsDTO)
        {
            // Add all our live ops from the save data
            foreach (LiveOpDTO liveOpDTO in liveOpsDTO.liveOps)
            {
                yield return liveOpsRecord.AddLiveOp(liveOpDTO);
            }

            yield return AddLiveOpsFromSchedule();
        }

        private IEnumerator AddLiveOpsFromSchedule()
        {
            // Add any missing liveops from our schedule too, but make sure they're starting now if they will be added
            foreach (LiveOpDTO liveOpDTO in liveOpsSchedule.Items)
            {
                yield return liveOpsRecord.AddLiveOp(liveOpDTO, GameTime.Now);
            }
        }
    }
}
