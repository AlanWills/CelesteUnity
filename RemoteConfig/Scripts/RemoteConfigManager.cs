using Celeste.Persistence;
using Celeste.RemoteConfig.Objects;
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
        [SerializeField] private RemoteConfigEnvironmentIds environmentIds;
#if UNITY_REMOTE_CONFIG
        [SerializeField] private DataSource defaultDataSource = DataSource.Unity;
#else
        [SerializeField] private DataSource defaultDataSource = DataSource.Disabled;
#endif
#if UNITY_EDITOR
        [Header("Editor Only")]
        [SerializeField] private bool editorOnly_FetchOnStart = true;
#endif

        #endregion
        
        #region Unity Methods

        protected override void Start()
        {
            base.Start();

#if UNITY_EDITOR
            if (editorOnly_FetchOnStart)
            {
                remoteConfigRecord.FetchData();
            }
#endif
        }

        #endregion

        #region Save/Load

        protected override void Deserialize(RemoteConfigManagerDTO dto)
        {
            DataSource dataSource = (DataSource)dto.dataSource;
            remoteConfigRecord.Initialize(dataSource, environmentIds);
            remoteConfigRecord.Deserialize(dto);
        }

        protected override RemoteConfigManagerDTO Serialize()
        {
            return new RemoteConfigManagerDTO(remoteConfigRecord);
        }

        protected override void SetDefaultValues()
        {
            remoteConfigRecord.Initialize(defaultDataSource, environmentIds);
        }

        #endregion
    }
}
