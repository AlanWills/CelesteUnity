#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Celeste.Tools
{
    public static class WebGLExtensions
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern void SyncFiles();
#else
        public static void SyncFiles(){}
#endif
    }
}
