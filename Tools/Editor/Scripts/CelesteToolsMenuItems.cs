using Celeste.Tools;
using UnityEditor;

namespace CelesteEditor.Tools
{
    public static class CelesteToolsMenuItems
    {
        [MenuItem("Celeste/Tools/Clear Cache")]
        public static void ClearCacheMenuItem()
        {
            CachingUtility.ClearCache();
        }
    }
}
