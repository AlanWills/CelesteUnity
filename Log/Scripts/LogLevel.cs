using System;
using UnityEngine;

namespace Celeste.Log
{
    [Serializable, Flags]
    public enum LogLevel
    {
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 4,
        Exception = 8,
        Assert = 16,
    }

    public static class LogLevelExtensions
    {
        public static LogLevel ToLogLevel(this LogType level)
        {
            switch (level)
            {
                case LogType.Log: return LogLevel.Info;
                case LogType.Warning: return LogLevel.Warning;
                case LogType.Error: return LogLevel.Error;
                case LogType.Exception: return LogLevel.Exception;
                case LogType.Assert: return LogLevel.Assert;
                default: return LogLevel.None;
            }
        }
    }
}