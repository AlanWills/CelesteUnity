using System;

namespace Celeste.Web
{
    public struct ProgressLogger : IProgress<string>
    {
        public void Report(string value)
        {
            UnityEngine.Debug.Log(value);
        }
    }
}