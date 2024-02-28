using Celeste.Parameters;
using Celeste.RemoteConfig.Persistence;
using FullSerializer;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.RemoteConfig
{
    public enum DataSource
    {
        Unity,
        Disabled,
    }

    [CreateAssetMenu(fileName = nameof(RemoteConfigRecord), order = CelesteMenuItemConstants.REMOTECONFIG_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.REMOTECONFIG_MENU_ITEM + "Remote Config Record")]
    public class RemoteConfigRecord : ScriptableObject, IRemoteConfigDictionary
    {
        #region Properties and Fields

        public DataSource DataSource
        {
            get => dataSource;
            set
            {
                if (dataSource != value)
                {
                    impl.RemoveOnDataFetchedCallback(OnDataFetched);

                    SetDataSourceInternal(value);

                    fetchedJson = string.Empty;
                    fetchedData = new fsDataDictionary();
                    fetchedDataChanged.Invoke();
                    save.Invoke();
                }
            }
        }

        [SerializeField] private BoolValue isDebugBuild;
        [SerializeField] private string unityProductionEnvironmentID;
        [SerializeField] private string unityDevelopmentEnvironmentID;
        [SerializeField] private Events.Event fetchedDataChanged;
        [SerializeField] private Events.Event save;

        [NonSerialized] private string environmentID;
        [NonSerialized] private string fetchedJson = string.Empty;
        [NonSerialized] private fsDataDictionary fetchedData = new fsDataDictionary();
        [NonSerialized] private IRemoteConfigImpl impl = new DisabledRemoteConfigImpl();
        [NonSerialized] private DataSource dataSource = DataSource.Disabled;

        #endregion

        public void Initialize(DataSource dataSource)
        {   
            SetDataSourceInternal(dataSource);
        }

        private void SetDataSourceInternal(DataSource dataSource)
        {
            this.dataSource = dataSource;

            switch (dataSource)
            {
                default:
                case DataSource.Disabled:
                    impl = new DisabledRemoteConfigImpl();
                    environmentID = "";
                    break;

#if UNITY_REMOTE_CONFIG
                case DataSource.Unity:
                    impl = new UnityRemoteConfigImpl();
                    environmentID = isDebugBuild.Value ? unityDevelopmentEnvironmentID : unityProductionEnvironmentID;
                    break;
#endif
            }

            impl.AddOnDataFetchedCallback(OnDataFetched);
            UnityEngine.Debug.Log($"Remote Config data source set to '{dataSource}'!");
        }

        public void Deserialize(RemoteConfigManagerDTO remoteConfigManagerDTO)
        {
            DeserializeData(remoteConfigManagerDTO.cachedConfig);
        }

        private void DeserializeData(string json)
        {
            if (string.IsNullOrEmpty(json) || json == "{}")
            {
                UnityEngine.Debug.LogWarning($"Received empty json for {nameof(RemoteConfigRecord)}.{nameof(DeserializeData)}.");
                fetchedJson = json;
                fetchedData = new fsDataDictionary();
            }
            else
            {
                fsResult parsingResult = fsJsonParser.Parse(fetchedJson, out fsData parsedData);

                if (parsingResult.Succeeded && parsedData.IsDictionary)
                {
                    fetchedJson = json;
                    fetchedData = new fsDataDictionary(parsedData.AsDictionary);
                }
                else
                {
                    UnityEngine.Debug.LogError($"Failed to parse non-empty json {fetchedJson} into a dictionary for {nameof(RemoteConfigRecord)}.{nameof(DeserializeData)}.");
                    fetchedJson = json;
                    fetchedData = new fsDataDictionary();
                }
            }
        }

        public async Task FetchData()
        {
            await impl.FetchData(environmentID);
        }

        public string ToJson()
        {
            return fetchedJson;
        }

        public bool GetBool(string key, bool defaultValue)
        {
            return fetchedData.GetBool(key, defaultValue);
        }

        public IRemoteConfigDictionary GetDictionary(string key)
        {
            return fetchedData.GetDictionary(key);
        }

        public string GetString(string key, string defaultValue)
        {
            return fetchedData.GetString(key, defaultValue);
        }

        public int GetInt(string key, int defaultValue)
        {
            return fetchedData.GetInt(key, defaultValue);
        }

        public float GetFloat(string key, float defaultValue)
        {
            return fetchedData.GetFloat(key, defaultValue);
        }

        #region Callbacks

        private void OnDataFetched(string data)
        {
            DeserializeData(data);
            
            fetchedDataChanged.Invoke();
            save.Invoke();
        }

        #endregion
    }
}
