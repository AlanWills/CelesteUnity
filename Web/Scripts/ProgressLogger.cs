using System;

namespace Celeste.Web
{
    public struct ProgressLogger : IProgress<string>
    {
        public static ProgressLogger Default => new ProgressLogger();
        
        public void Report(string value)
        {
            UnityEngine.Debug.Log(value);
        }
    }
}