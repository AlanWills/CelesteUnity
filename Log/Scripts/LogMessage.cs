using System;

namespace Celeste.Log
{
    [Serializable]
    public struct LogMessage
    {
        public string message;
        public string stackTrace;
        public LogLevel logType;
    }
}
