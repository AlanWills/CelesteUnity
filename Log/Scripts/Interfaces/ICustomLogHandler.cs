using System;
using UnityEngine;

namespace Celeste.Log
{
    public interface ICustomLogHandler
    {
        [HideInCallstack]
        void Log(LogType logType, UnityEngine.Object context, string message, string stackTrace);
        
        [HideInCallstack]
        void LogException(Exception exception, UnityEngine.Object context, string message);
    }
}