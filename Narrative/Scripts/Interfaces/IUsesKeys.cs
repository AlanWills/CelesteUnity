namespace Celeste.Narrative
{
    public interface IKey
    { 
        string Key { get; }
    }

    public interface IUsesKeys
    {
        bool UsesKey(IKey key);
        bool CouldUseKey(IKey key);
        void AddKeyForUse(IKey key);
    }
}
