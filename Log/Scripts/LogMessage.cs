using System;

namespace Celeste.Log
{
    [Serializable]
    public struct LogMessage
    {
        public string message;
        public string trackTrace;
        public LogLevel logType;
    }
}
