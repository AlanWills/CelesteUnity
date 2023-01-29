using Celeste.Advertising.Persistence;
using Celeste.Events;
using Celeste.Parameters;
using Celeste.Persistence;
using UnityEngine;

namespace Celeste.Advertising
{
    public class AdsManager : PersistentSceneManager<AdsManager, AdsDTO>
    {
        #region Properties and Fields

        public const string FILE_NAME = "Ads.dat";
        protected override string FileName => FILE_NAME;

        [SerializeField] private BoolValue isDebugBuild;
        [SerializeField] private BoolValue adTestMode;
        [SerializeField] private AdRecord adRecord;

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            Load();

            // Only enter test mode if we have enabled the flag and we are in a debug build
            // We should never be able to allow test mode on release builds, even if the flag is on for some reason
            adRecord.Initialize(adTestMode.Value && isDebugBuild.Value);
        }

        #endregion

        #region Save/Load

        protected override AdsDTO Serialize()
        {
            return new AdsDTO(adTestMode.Value);
        }

        protected override void Deserialize(AdsDTO dto)
        {
            adTestMode.Value = dto.adsTestMode;
        }

        protected override void SetDefaultValues()
        {
            adTestMode.Value = isDebugBuild.Value;
        }

        #endregion

        #region Callbacks

        public void OnAdTestModeChanged(ValueChangedArgs<bool> args)
        {
            Save();
        }

        #endregion
    }
}
