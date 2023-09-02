﻿namespace Celeste.RemoteConfig
{
    public interface IRemoteConfigDictionary
    {
        bool GetBool(string key, bool defaultValue);
        string GetString(string key, string defaultValue);
        int GetInt(string key, int defaultValue);
        float GetFloat(string key, float defaultValue);
        IRemoteConfigDictionary GetDictionary(string key);
    }
}
