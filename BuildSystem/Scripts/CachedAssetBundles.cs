using System;
using System.Collections.Generic;

namespace Celeste.BuildSystem
{
    [Serializable]
    public class CachedAssetBundles
    {
        public bool IsValid => cachedBundleList != null && cachedBundleList.Count > 0;

        public List<string> cachedBundleList = new List<string>();
    }
}
