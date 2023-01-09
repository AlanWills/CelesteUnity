using Celeste.Tools;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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
