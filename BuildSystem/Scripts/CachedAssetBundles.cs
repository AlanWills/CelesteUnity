using System;
using System.Collections.Generic;

namespace Celeste.BuildSystem
{
    [Serializable]
    public class CachedAssetBundles
    {
        public bool IsValid => string.IsNullOrEmpty(cacheLocation) && cachedBundleList != null && cachedBundleList.Count > 0;

        public string cacheLocation;
        public List<string> cachedBundleList = new List<string>();
    }
}
