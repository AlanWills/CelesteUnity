using Celeste.Persistence;
using Celeste.RemoteConfig.Persistence;
using UnityEngine;

namespace Celeste.RemoteConfig
{
    [AddComponentMenu("Celeste/Remote Config/Remote Config Manager")]
    public class RemoteConfigManager : PersistentSceneManager<RemoteConfigManager, RemoteConfigManagerDTO>
    {
        #region Properties and Fields

        public const string FILE_NAME = "RemoteConfig.dat";
        protected override string FileName => FILE_NAME;

        [SerializeField] private RemoteConfigRecord remoteConfigRecord;
#if UNITY_REMOTE_CONFIG
        [SerializeField] private DataSource defaultDataSource = DataSource.Unity;
#else
        [SerializeField] private DataSource defaultDataSource = DataSource.Disabled;
#endif

        #endregion

        #region Save/Load

        protected override void Deserialize(RemoteConfigManagerDTO dto)
        {
            remoteConfigRecord.Initialize((DataSource)dto.dataSource);
        }

        protected override RemoteConfigManagerDTO Serialize()
        {
            return new RemoteConfigManagerDTO(remoteConfigRecord);
        }

        protected override void SetDefaultValues()
        {
            remoteConfigRecord.Initialize(defaultDataSource);
        }

        #endregion
    }
}
