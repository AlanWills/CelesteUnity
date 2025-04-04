using Celeste.Tools;
using FullSerializer;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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

            byte[] serializedBytes = File.ReadAllBytes(filePath);
            string serializedJson = Encoding.UTF8.GetString(serializedBytes);
            var deserializeResult = Deserialize<T>(serializedJson);

            return deserializeResult.Item1.Failed ? default : deserializeResult.Item2;
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
                    byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
                    File.WriteAllBytes(filePath, bytes);
                }
                
#if UNITY_EDITOR
                // Save debug human readable file
                {
                    string debugPersistentFilePath = $"{filePath}.{PersistenceConstants.DEBUG_FILE_EXTENSION}";
                    
                    if (result.Succeeded)
                    {
                        string jsonData = fsJsonPrinter.PrettyJson(data);
                        File.WriteAllText(debugPersistentFilePath, jsonData);
                    }
                }
#endif
            }
            
            // Needed to deal with browser async saving
            WebGLExtensions.SyncFiles();
        }

        public static async Task SaveAsync<T>(string filePath, T persistenceDTO)
        {
            // Is async file saving possible in WebGL?

            // Save binary file
            {
                // Serialize the data
                fsResult result = serializer.TrySerialize(persistenceDTO, out fsData data);

                if (result.Succeeded)
                {
                    string jsonData = fsJsonPrinter.CompressedJson(data);
                    byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
                    await File.WriteAllBytesAsync(filePath, bytes);
                }
                
#if UNITY_EDITOR
                // Save debug human readable file
                {
                    string debugPersistentFilePath = $"{filePath}.{PersistenceConstants.DEBUG_FILE_EXTENSION}";
                    
                    if (result.Succeeded)
                    {
                        string jsonData = fsJsonPrinter.PrettyJson(data);
                        await File.WriteAllTextAsync(debugPersistentFilePath, jsonData);
                    }
                }
#endif
            }
            
            // Needed to deal with browser async saving
            WebGLExtensions.SyncFiles();
        }
        
        #endregion

        public static string Serialize(object obj)
        {
            // Serialize the data
            fsResult result = serializer.TrySerialize(obj.GetType(), obj, out fsData data);
            return result.Succeeded ? fsJsonPrinter.CompressedJson(data) : string.Empty;
        }

        public static ValueTuple<fsResult, T> Deserialize<T>(string json)
        {
            fsResult parseResult = fsJsonParser.Parse(json, out fsData data);
            
            if (!parseResult.Succeeded)
            {
                return new ValueTuple<fsResult, T>(parseResult, default);
            }

            T obj = default;
            fsResult deserializeResult = serializer.TryDeserialize(data, ref obj);

            return new ValueTuple<fsResult, T>(deserializeResult, obj);
        }

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
