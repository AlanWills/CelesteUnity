namespace Celeste.Tools
{
    public static class WebGLUtils
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern void SyncFiles();
#else
        public static void SyncFiles(){}
#endif
    }
}
