using Celeste.Parameters;
using Celeste.Persistence;
using Celeste.RemoteConfig.Persistence;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;

namespace Celeste.RemoteConfig
{
    [AddComponentMenu("Celeste/Remote Config/Remote Config Manager")]
    public class RemoteConfigManager : PersistentSceneManager<RemoteConfigManager, RemoteConfigManagerDTO>
    {
        #region Properties and Fields

        public const string FILE_NAME = "RemoteConfig.dat";
        protected override string FileName => FILE_NAME;

        private string EnvironmentID => isDebugBuild.Value ? developmentEnvironmentID : productionEnvironmentID;

        [SerializeField] private RemoteConfigRecord remoteConfigRecord;
        [SerializeField] private BoolValue isDebugBuild;
        [SerializeField] private bool useUnityRemoteConfig;
        [SerializeField, ShowIf(nameof(useUnityRemoteConfig))] private string productionEnvironmentID;
        [SerializeField, ShowIf(nameof(useUnityRemoteConfig))] private string developmentEnvironmentID;

        private IRemoteConfigImpl impl = new DisabledRemoteConfigImpl();

        #endregion

        #region Save/Load

        protected override void Deserialize(RemoteConfigManagerDTO dto)
        {
            remoteConfigRecord.FromJson(dto.cachedConfig);
        }

        protected override RemoteConfigManagerDTO Serialize()
        {
            return new RemoteConfigManagerDTO(remoteConfigRecord);
        }

        protected override void SetDefaultValues()
        {
        }

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            loadOnAwake = false;
            loadOnStart = false;
        }

        protected override void Awake()
        {
            Load();

            base.Awake();

            if (useUnityRemoteConfig)
            {
                impl = new UnityRemoteConfigImpl();
            }

            impl.AddOnDataFetchedCallback(OnDataFetched);
            impl.FetchData(EnvironmentID);
        }

        protected override void OnDestroy()
        {
            impl.RemoveOnDataFetchedCallback(OnDataFetched);

            base.OnDestroy();
        }

        #endregion

        #region Callbacks

        private void OnDataFetched(string data)
        {
            remoteConfigRecord.FromJson(data);
        }

        #endregion
    }
}
