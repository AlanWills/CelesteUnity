using FullSerializer;
using System.Collections.Generic;

namespace Celeste.RemoteConfig
{
    public class fsDataDictionary : IRemoteConfigDictionary
    {
        private Dictionary<string, fsData> dictionary;

        public fsDataDictionary() : this(new Dictionary<string, fsData>())
        {
        }

        public fsDataDictionary(Dictionary<string, fsData> dictionary)
        {
            this.dictionary = dictionary;
        }

        public bool GetBool(string key, bool defaultValue)
        {
            if (dictionary.TryGetValue(key, out fsData value))
            {
                return value.IsBool ? value.AsBool : defaultValue;
            }
            
            return defaultValue;
        }

        public float GetFloat(string key, float defaultValue)
        {
            if (dictionary.TryGetValue(key, out fsData value))
            {
                return value.IsDouble ? (float)value.AsDouble : defaultValue;
            }

            return defaultValue;
        }

        public int GetInt(string key, int defaultValue)
        {
            if (dictionary.TryGetValue(key, out fsData value))
            {
                return value.IsInt64 ? (int)value.AsInt64 : defaultValue;
            }

            return defaultValue;
        }

        public string GetString(string key, string defaultValue)
        {
            if (dictionary.TryGetValue(key, out fsData value))
            {
                return value.IsString ? value.AsString : defaultValue;
            }

            return defaultValue;
        }

        public IRemoteConfigDictionary GetDictionary(string key)
        {
            if (dictionary.TryGetValue(key, out fsData value))
            {
                return new fsDataDictionary(value.IsDictionary ? value.AsDictionary : new Dictionary<string, fsData>());
            }

            return new fsDataDictionary();
        }
    }
}
 