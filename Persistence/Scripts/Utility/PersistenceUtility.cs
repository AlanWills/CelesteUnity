using Celeste.Log;
using Celeste.OdinSerializer;
using Celeste.Tools;
using System.IO;
using UnityEngine;

namespace Celeste.Persistence
{
    public static class PersistenceUtility
    {
        #region Save/Load Methods

        public static bool CanLoad(string persistentFilePath)
        {
            return File.Exists(persistentFilePath);
        }

        public static T Load<T>(string filePath)
        {
            if (!CanLoad(filePath))
            {
                return default;
            }

            byte[] bytes = File.ReadAllBytes(filePath);
            T persistenceDTO = SerializationUtility.DeserializeValue<T>(bytes, DataFormat.Binary);

            return persistenceDTO;
        }

        public static void Save<T>(string filePath, T persistenceDTO)
        {
            // Save binary file
            {
                byte[] bytes = SerializationUtility.SerializeValue(persistenceDTO, DataFormat.Binary);
                File.WriteAllBytes(filePath, bytes);
            }

#if UNITY_EDITOR
            // Save debug human readable file
            {
                string debugPersistentFilePath = $"{filePath}.{PersistenceConstants.DEBUG_FILE_EXTENSION}";
                byte[] json = SerializationUtility.SerializeValue(persistenceDTO, DataFormat.JSON);
                File.WriteAllBytes(debugPersistentFilePath, json);
            }
#endif

            // Needed to deal with browser async saving
            WebGLUtils.SyncFiles();
        }

        #endregion

        public static void DeletePersistentDataFile(string fileNameAndExtension)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileNameAndExtension);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
#if UNITY_EDITOR
                File.Delete($"{filePath}.{PersistenceConstants.DEBUG_FILE_EXTENSION}");
#endif
                UnityEngine.Debug.Log($"Deleted save file at {filePath}.");
            }
        }
    }
}
