using System;
using UnityEngine;

namespace Celeste.Log
{
    [Serializable]
    public struct LogMessage
    {
        public string message;
        public string stackTrace;
        public LogLevel logType;
        [NonSerialized] public SectionLogSettings sectionLogSettings;
    }
    
    public struct RawUnityLog
    {
        public LogType logType;
        public UnityEngine.Object context;
        public string format;
        public object[] args;
    }
}
