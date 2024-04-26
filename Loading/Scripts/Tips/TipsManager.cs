using Celeste.Persistence;
using UnityEngine;

namespace Celeste.Loading
{
    [AddComponentMenu("Celeste/Loading/Tips/Tips Manager")]
    public class TipsManager : PersistentSceneManager<TipsManager, TipsManagerDTO>
    {
        #region Properties and Fields

        public const string FILE_NAME = "Tips.dat";
        protected override string FileName => FILE_NAME;

        [SerializeField] private TipsRecord tipsRecord;

        #endregion

        #region Save/Load Methods

        protected override TipsManagerDTO Serialize()
        {
            return new TipsManagerDTO(tipsRecord);
        }

        protected override void Deserialize(TipsManagerDTO tipsManagerDTO)
        {
            tipsRecord.Initialize(tipsManagerDTO.unseenIndexes, tipsManagerDTO.seenIndexes);
        }

        protected override void SetDefaultValues()
        {
            tipsRecord.Initialize();
        }

        #endregion
    }
}
