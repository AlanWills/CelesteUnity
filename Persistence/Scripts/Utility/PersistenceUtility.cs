using Celeste.Log;
using Celeste.OdinSerializer;
using Celeste.Tools;
using FullSerializer;
using System.IO;
using UnityEngine;

namespace Celeste.Persistence
{
    public static class PersistenceUtility
    {
        #region Properties and Fields
        
        private static readonly fsSerializer serializer = new fsSerializer();

        #endregion

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

            string serializedState = File.ReadAllText(filePath);

            // Step 1: parse the JSON data
            fsResult parseResult = fsJsonParser.Parse(serializedState, out fsData data);
            if (parseResult.Failed)
            {
                return default;
            }

            // Step 2: deserialize the data
            T deserialized = default;
            fsResult deserializeResult = serializer.TryDeserialize(data, ref deserialized);

            if (deserializeResult.Failed)
            {
                return default;
            }

            return deserialized;
        }

        public static void Save<T>(string filePath, T persistenceDTO)
        {
            // Save binary file
            {
                // Serialize the data
                fsResult result = serializer.TrySerialize(persistenceDTO, out fsData data);

                if (result.Succeeded)
                {
                    string jsonData = fsJsonPrinter.CompressedJson(data);
                    File.WriteAllText(filePath, jsonData);
                }
            }

#if UNITY_EDITOR
            // Save debug human readable file
            {
                string debugPersistentFilePath = $"{filePath}.{PersistenceConstants.DEBUG_FILE_EXTENSION}";
                fsResult result = serializer.TrySerialize(persistenceDTO, out fsData data);
                
                if (result.Succeeded)
                {
                    string jsonData = fsJsonPrinter.PrettyJson(data);
                    File.WriteAllText(filePath, jsonData);
                }
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
