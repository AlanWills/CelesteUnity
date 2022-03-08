using UnityEngine;

namespace Celeste.Tools
{
    public static class CachingUtility
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
        }
    }
}
