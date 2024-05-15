using System;
using UnityEngine;

namespace Celeste.Log
{
    public class HudLogHandler : ICustomLogHandler
    {
        public void Log(LogType logType, UnityEngine.Object context, string message)
        {
            switch (logType)
            {
                case LogType.Log:
                    HudLog.LogInfo(message); break;
                case LogType.Warning:
                    HudLog.LogWarning(message); break;
                case LogType.Error:
                    HudLog.LogError(message); break;
                case LogType.Assert:
                    HudLog.LogError(message); break;
                case LogType.Exception:
                    HudLog.LogError(message); break;
            }
        }

        public void LogException(Exception exception, UnityEngine.Object context, string message)
        {
            HudLog.LogError($"{message}");
        }
    }
}