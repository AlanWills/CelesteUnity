using Celeste.Parameters;
using Celeste.RemoteConfig.Persistence;
using FullSerializer;
using System;
using System.Threading.Tasks;
using Celeste.DataStructures;
using Celeste.RemoteConfig.Objects;
using UnityEngine;

namespace Celeste.RemoteConfig
{
    public enum DataSource
    {
        Disabled,
#if UNITY_REMOTE_CONFIG
        Unity = 1
#endif
    }

    [CreateAssetMenu(fileName = nameof(RemoteConfigRecord), order = CelesteMenuItemConstants.REMOTECONFIG_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.REMOTECONFIG_MENU_ITEM + "Remote Config Record")]
    public class RemoteConfigRecord : ScriptableObject, IDataDictionary
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
        [SerializeField] private Events.Event fetchedDataChanged;
        [SerializeField] private Events.Event save;

        [NonSerialized] private RemoteConfigEnvironmentIds environmentIDs;
        [NonSerialized] private string fetchedJson = string.Empty;
        [NonSerialized] private fsDataDictionary fetchedData = new fsDataDictionary();
        [NonSerialized] private IRemoteConfigImpl impl = new DisabledRemoteConfigImpl();
        [NonSerialized] private DataSource dataSource = DataSource.Disabled;

        #endregion

        public void Initialize(DataSource dataSource, RemoteConfigEnvironmentIds environmentIds)
        {
            environmentIDs = environmentIds;
            
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
                    break;

#if UNITY_REMOTE_CONFIG
                case DataSource.Unity:
                    impl = new UnityRemoteConfigImpl();
                    break;
#endif
            }

            impl.AddOnDataFetchedCallback(OnDataFetched);
            UnityEngine.Debug.Log($"Remote Config data source set to '{dataSource}'!", CelesteLog.RemoteConfig.WithContext(this));
        }

        public void Deserialize(RemoteConfigManagerDTO remoteConfigManagerDTO)
        {
            DeserializeData(remoteConfigManagerDTO.cachedConfig);
            
            fetchedDataChanged.Invoke();
        }

        private void DeserializeData(string json)
        {
            if (string.IsNullOrEmpty(json) || json == "{}")
            {
                UnityEngine.Debug.LogWarning($"Received empty json for {nameof(RemoteConfigRecord)}.{nameof(DeserializeData)}.", CelesteLog.RemoteConfig.WithContext(this));
                fetchedJson = json;
                fetchedData = new fsDataDictionary();
            }
            else
            {
                fsResult parsingResult = fsJsonParser.Parse(json, out fsData parsedData);

                if (parsingResult.Succeeded && parsedData.IsDictionary)
                {
                    UnityEngine.Debug.Log($"Fetched '{json}' from remote config successfully!", CelesteLog.RemoteConfig.WithContext(this));
                    fetchedJson = json;
                    fetchedData = new fsDataDictionary(parsedData.AsDictionary);
                }
                else
                {
                    UnityEngine.Debug.LogError($"Failed to parse non-empty json '{json}' into a dictionary for {nameof(RemoteConfigRecord)}.{nameof(DeserializeData)}.", CelesteLog.RemoteConfig.WithContext(this));
                    fetchedJson = json;
                    fetchedData = new fsDataDictionary();
                }
            }
        }

        public async Task FetchData()
        {
            await impl.FetchData(environmentIDs, isDebugBuild.Value);
        }

        public string ToJson()
        {
            return fetchedJson;
        }

        public bool GetBool(string key, bool defaultValue)
        {
            return fetchedData.GetBool(key, defaultValue);
        }

        public IDataDictionary GetObjectAsDictionary(string key)
        {
            return fetchedData.GetObjectAsDictionary(key);
        }

        public string GetObjectAsString(string key, string defaultValue)
        {
            return fetchedData.GetObjectAsString(key, defaultValue);
        }

        public T GetObject<T>(string key, T defaultValue)
        {
            string objectAsString = fetchedData.GetObjectAsString(key, string.Empty);
            if (string.IsNullOrEmpty(objectAsString))
            {
                return defaultValue;
            }

            T deserializedObject = JsonUtility.FromJson<T>(objectAsString);
            return (deserializedObject == null || deserializedObject.Equals(default(T))) ? defaultValue : deserializedObject;
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
