using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    public interface IUsesKeys
    {
        bool UsesKey(string key);
        bool CouldUseKey(string key, object obj);
        void AddKeyForUse(string key, object obj);
    }
}
