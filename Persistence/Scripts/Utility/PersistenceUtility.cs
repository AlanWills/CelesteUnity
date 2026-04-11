using Celeste.Tools;
using FullSerializer;
using System;
using System.Collections.Generic;
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
            
            string serializedJson = File.ReadAllText(filePath);
            var deserializeResult = Deserialize<T>(serializedJson);

            return deserializeResult.Item1.Failed ? default : deserializeResult.Item2;
        }

        public static void Save<T>(string filePath, T persistenceDto)
        {
            // Save binary file
            {
                // Serialize the data
                fsResult result = serializer.TrySerialize(persistenceDto, out fsData data);

                if (result.Succeeded)
                {
                    string jsonData = fsJsonPrinter.CompressedJson(data);
                    File.WriteAllText(filePath, jsonData);
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

        public static void WriteTextToFile(string filePath, string fileContents)
        {
            File.WriteAllText(filePath, fileContents);
            WebGLExtensions.SyncFiles();
        }
        
        public static async Task SaveAsync<T>(string filePath, T persistenceDto)
        {
#if UNITY_WEGL
            // Async saving is not possible in WebGL due to issues with File.WriteAllTextAsync etc.
            // So we fake it here
            // We have to do SyncFiles to deal with the browser async saving
            Save(filePath, persistenceDTO);
            WebGLExtensions.SyncFiles();
            await Task.Yield();
#else
            
            // Save binary file
            {
                // Serialize the data
                fsResult result = serializer.TrySerialize(persistenceDto, out fsData data);

                if (result.Succeeded)
                {
                    string jsonData = fsJsonPrinter.CompressedJson(data);
                    await File.WriteAllTextAsync(filePath, jsonData);
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
#endif
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

        public static ValueTuple<fsResult, List<fsData>> DeserializeList(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return new ValueTuple<fsResult, List<fsData>>(fsResult.Warn("Inputted json was empty or whitespace!"), new List<fsData>());
            }
            
            fsResult parseResult = fsJsonParser.Parse(json, out fsData data);
            
            if (!parseResult.Succeeded)
            {
                return new ValueTuple<fsResult, List<fsData>>(parseResult, new List<fsData>());
            }

            if (!data.IsList)
            {
                return new ValueTuple<fsResult, List<fsData>>(parseResult, new List<fsData>());
            }

            return new ValueTuple<fsResult, List<fsData>>(parseResult, data.AsList);
        }

        public static ValueTuple<fsResult, Dictionary<string, fsData>> DeserializeObject(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return new ValueTuple<fsResult, Dictionary<string, fsData>>(fsResult.Warn("Inputted json was empty or whitespace!"),
                    new Dictionary<string, fsData>(StringComparer.Ordinal));
            }
            
            fsResult parseResult = fsJsonParser.Parse(json, out fsData data);
            
            if (!parseResult.Succeeded)
            {
                return new ValueTuple<fsResult, Dictionary<string, fsData>>(parseResult, new Dictionary<string, fsData>(StringComparer.Ordinal));
            }

            if (!data.IsDictionary)
            {
                return new ValueTuple<fsResult, Dictionary<string, fsData>>(parseResult, new Dictionary<string, fsData>(StringComparer.Ordinal));
            }

            return new ValueTuple<fsResult, Dictionary<string, fsData>>(parseResult, data.AsDictionary);
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

        public static void DeletePersistentDataFolder(string relativeFolderPath)
        {
            string folderPath = Path.Combine(Application.persistentDataPath, relativeFolderPath);
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath);
                UnityEngine.Debug.Log($"Deleted folder at {folderPath}.");
            }
        }
    }
}
