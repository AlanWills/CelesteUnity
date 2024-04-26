using System.IO;
using UnityEngine;

namespace Celeste.Tools
{
    public static class CachingExtensions
    {
        public static void ClearCache()
        {
            if (Caching.ClearCache())
            {
                Debug.Log("Cache cleared successfully!");
            }
            else
            {
                Debug.LogError("Cache could not be cleared!");
            }

            string pathToAddressablesDirectory = Path.Combine(Application.persistentDataPath, "com.unity.addressables");
            if (Directory.Exists(pathToAddressablesDirectory))
            {
                Directory.Delete(pathToAddressablesDirectory, true);
                Debug.Log("Cleared addressables cache in persistent data successfully!");
            }
            else
            {
                Debug.Log($"Skipping clearing of addressables cache in persistent data as it does not exist.");
            }
        }
    }
}
