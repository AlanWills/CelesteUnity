using Celeste.Log;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Celeste.Persistence
{
    public static class PersistenceUtility<T> where T : class, new()
    {
        #region Save/Load Methods

        public static bool CanLoad(string persistentFilePath)
        {
            return File.Exists(persistentFilePath);
        }

        public static T Load(string persistentFilePath)
        {
            if (!CanLoad(persistentFilePath))
            {
                return null;
            }

            using (FileStream fileStream = new FileStream(persistentFilePath, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                string persistenceString = bf.Deserialize(fileStream) as string;

                T persistenceDTO = new T();
                JsonUtility.FromJsonOverwrite(persistenceString, persistenceDTO);
                HudLog.LogInfo($"{nameof(T)} loaded");

                return persistenceDTO;
            }
        }

        public static void Save(string filePath, T persistenceDTO)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                string persistenceString = JsonUtility.ToJson(persistenceDTO);

                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fileStream, persistenceString);
                HudLog.LogInfo($"{nameof(T)} saved");
            }
        }

        #endregion
    }
}
