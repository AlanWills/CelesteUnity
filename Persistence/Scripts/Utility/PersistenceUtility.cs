using Celeste.Log;
using Celeste.OdinSerializer;
using Celeste.Tools;
using System.IO;

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
    }
}
