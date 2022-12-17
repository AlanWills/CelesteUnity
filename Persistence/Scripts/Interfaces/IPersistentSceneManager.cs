namespace Celeste.Persistence
{
    public interface IPersistentSceneManager
    {
        void Load();
        void Save();

        string SerializeToString();
    }
}
