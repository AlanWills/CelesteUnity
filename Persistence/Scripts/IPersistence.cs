namespace Celeste.Persistence
{
    public interface IPersistence<T> where T : class, new()
    {
        bool CanLoad(string persistentFilePath);
        T Load(string persistentFilePath);
        void Save(string filePath, T campaignPersistenceDTO);
    }
}