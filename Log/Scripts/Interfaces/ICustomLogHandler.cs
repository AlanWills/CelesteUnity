using System;
using UnityEngine;

namespace Celeste.Log
{
    public interface ICustomLogHandler
    {
        void Log(LogType logType, UnityEngine.Object context, string message, string stackTrace);
        void LogException(Exception exception, UnityEngine.Object context, string message);
    }
}