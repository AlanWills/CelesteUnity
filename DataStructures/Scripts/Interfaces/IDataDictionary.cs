namespace Celeste.DataStructures
{
    public interface IDataDictionary
    {
        bool GetBool(string key, bool defaultValue);
        string GetString(string key, string defaultValue);
        int GetInt(string key, int defaultValue);
        float GetFloat(string key, float defaultValue);
        IDataDictionary GetObjectAsDictionary(string key);
        string GetObjectAsString(string key, string defaultValue);

        string ToString();
    }
}
