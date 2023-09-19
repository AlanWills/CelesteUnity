using FullSerializer;
using UnityEngine;

namespace Celeste.RemoteConfig
{
    [CreateAssetMenu(fileName = nameof(RemoteConfigRecord), menuName = "Celeste/Remote Config/Remote Config Record")]
    public class RemoteConfigRecord : ScriptableObject, IRemoteConfigDictionary
    {
        #region Properties and Fields

        [SerializeField] private Events.Event fetchedDataChanged;

        private string fetchedJson = string.Empty;
        private fsDataDictionary fetchedData = new fsDataDictionary();

        #endregion

        public void FromJson(string json)
        {
            if (string.IsNullOrEmpty(json) || json == "{}")
            {
                UnityEngine.Debug.LogWarning($"Received empty json for {nameof(RemoteConfigRecord)}.{nameof(FromJson)}.");
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
                    UnityEngine.Debug.LogError($"Failed to parse non-empty json {fetchedJson} into a dictionary for {nameof(RemoteConfigRecord)}.{nameof(FromJson)}.");
                    fetchedJson = json;
                    fetchedData = new fsDataDictionary();
                }
            }

            fetchedDataChanged.Invoke();
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
    }
}
