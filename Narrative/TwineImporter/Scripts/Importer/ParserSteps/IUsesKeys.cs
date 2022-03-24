using UnityEngine;

namespace Celeste.Narrative.TwineImporter.ParserSteps
{
    public interface IUsesKeys<T>
    {
        void AddKey(string key, T obj);
        bool UsesKey(string key);
    }
}
