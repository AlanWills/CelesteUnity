using System;
using UnityEngine;

namespace Celeste.Log
{
    public class HudLogHandler : ICustomLogHandler
    {
        public void Log(LogType logType, UnityEngine.Object context, string message, string stackTrace)
        {
            switch (logType)
            {
                case LogType.Log:
                    HudLog.LogInfo(message, stackTrace); break;
                case LogType.Warning:
                    HudLog.LogWarning(message, stackTrace); break;
                case LogType.Error:
                    HudLog.LogError(message, stackTrace); break;
                case LogType.Assert:
                    HudLog.LogAssertion(message, stackTrace); break;
                case LogType.Exception:
                    HudLog.LogException(message, stackTrace); break;
            }
        }

        public void LogException(Exception exception, UnityEngine.Object context, string message)
        {
            HudLog.LogException(message, exception.StackTrace);
        }
    }
}