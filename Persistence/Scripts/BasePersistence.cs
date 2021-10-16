using Celeste.Log;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Celeste.Persistence
{
    public class BasePersistence<T> : IPersistence<T> where T : class, new()
    {
        #region Save/Load Methods

        public virtual bool CanLoad(string persistentFilePath)
        {
            return PersistenceUtility<T>.CanLoad(persistentFilePath);
        }

        public virtual T Load(string persistentFilePath)
        {
            return PersistenceUtility<T>.Load(persistentFilePath);
        }

        public virtual void Save(string filePath, T persistenceDTO)
        {
            PersistenceUtility<T>.Save(filePath, persistenceDTO);
        }

        #endregion
    }
}
