using System.Collections.Generic;

namespace Celeste.Narrative
{
    public interface IKey
    { 
        string Key { get; }
    }

    public interface IUsesKeys
    {
        IEnumerable<string> Keys { get; }

        bool UsesKey(IKey key);
        bool CouldUseKey(IKey key);
        void AddKeyForUse(IKey key);
    }
}
