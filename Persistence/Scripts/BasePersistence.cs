namespace Celeste.Persistence
{
    public class BasePersistence<T> : IPersistence<T> where T : class, new()
    {
        #region Save/Load Methods

        public virtual bool CanLoad(string persistentFilePath)
        {
            return PersistenceUtility.CanLoad(persistentFilePath);
        }

        public virtual T Load(string persistentFilePath)
        {
            return PersistenceUtility.Load<T>(persistentFilePath);
        }

        public virtual void Save(string filePath, T persistenceDTO)
        {
            PersistenceUtility.Save(filePath, persistenceDTO);
        }

        #endregion
    }
}
