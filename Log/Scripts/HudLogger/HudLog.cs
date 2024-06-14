using Celeste.Log.DataStructures;
using Celeste.Memory;
using Celeste.Objects;
using System;
using UnityEngine;

namespace Celeste.Log
{
    [AddComponentMenu("Celeste/Log/Hud Log")]
    public class HudLog : SceneSingleton<HudLog>
    {
        #region Properties and Fields

        private LogLevel LogLevel
        {
            get
            {
                if (!logLevelInitialized)
                {
                    logLevel = (LogLevel)PlayerPrefs.GetInt(HUD_LOG_LEVEL_PREFS_KEY, (int)defaultLogLevel);
                }

                return logLevel;
            }
            set
            {
                logLevel = value;
                PlayerPrefs.SetInt(HUD_LOG_LEVEL_PREFS_KEY, (int)logLevel);
                PlayerPrefs.Save();
            }
        }

        [SerializeField] private LogLevel defaultLogLevel = LogLevel.Assert | LogLevel.Exception | LogLevel.Error | LogLevel.Warning;
        [SerializeField] private LogMessageList logMessages;
        [SerializeField] private GameObjectAllocator hudMessages;
        [SerializeField] private Color infoColour = Color.white;
        [SerializeField] private Color warningColour = Color.yellow;
        [SerializeField] private Color errorColour = Color.red;

        [NonSerialized] private LogLevel logLevel;
        [NonSerialized] private bool logLevelInitialized;

        private const string HUD_LOG_LEVEL_PREFS_KEY = "HudLogLevel";

        #endregion

        #region Log Type

        public static bool IsLogLevelEnabled(LogLevel desiredLevel)
        {
            return Instance != null && (Instance.LogLevel & desiredLevel) == desiredLevel;
        }

        public static void AddLogLevel(LogLevel desiredLevel)
        {
            Instance.LogLevel |= desiredLevel;
        }

        public static void RemoveLogLevel(LogLevel desiredLevel)
        {
            Instance.LogLevel &= ~desiredLevel;
        }

        #endregion

        #region Logging Methods

        public static void LogInfo(string message)
        {
            if (Instance != null && IsLogLevelEnabled(LogLevel.Info))
            {
                Instance.Log(message, Instance.infoColour, LogLevel.Info);
            }
        }

        public static void LogInfo(string message, string stackTrace)
        {
            if (Instance != null && IsLogLevelEnabled(LogLevel.Info))
            {
                Instance.Log(message, stackTrace, Instance.infoColour, LogLevel.Info);
            }
        }

        public static void LogWarning(string message)
        {
            if (Instance != null && IsLogLevelEnabled(LogLevel.Warning))
            {
                Instance.Log(message, Instance.warningColour, LogLevel.Warning);
            }
        }

        public static void LogWarning(string message, string stackTrace)
        {
            if (Instance != null && IsLogLevelEnabled(LogLevel.Warning))
            {
                Instance.Log(message, stackTrace, Instance.warningColour, LogLevel.Warning);
            }
        }

        public static void LogError(string message)
        {
            if (Instance != null && IsLogLevelEnabled(LogLevel.Error))
            {
                Instance.Log(message, Instance.errorColour, LogLevel.Error);
            }
        }

        public static void LogError(string message, string stackTrace)
        {
            if (Instance != null && IsLogLevelEnabled(LogLevel.Error))
            {
                Instance.Log(message, stackTrace, Instance.errorColour, LogLevel.Error);
            }
        }

        public static void LogException(string message)
        {
            if (Instance != null && IsLogLevelEnabled(LogLevel.Exception))
            {
                Instance.Log(message, Instance.errorColour, LogLevel.Exception);
            }
        }

        public static void LogException(string message, string stackTrace)
        {
            if (Instance != null && IsLogLevelEnabled(LogLevel.Exception))
            {
                Instance.Log(message, stackTrace, Instance.errorColour, LogLevel.Exception);
            }
        }

        public static void LogAssertion(string message)
        {
            if (Instance != null && IsLogLevelEnabled(LogLevel.Assert))
            {
                Instance.Log(message, Instance.errorColour, LogLevel.Assert);
            }
        }

        public static void LogAssertion(string message, string stackTrace)
        {
            if (Instance != null && IsLogLevelEnabled(LogLevel.Assert))
            {
                Instance.Log(message, stackTrace, Instance.errorColour, LogLevel.Assert);
            }
        }

        public static void Clear()
        {
            if (Instance != null)
            {
                if (Instance.logMessages != null)
                {
                    Instance.logMessages.ClearItems();
                }

                Instance.hudMessages.DeallocateAll();
            }
        }

        private void Log(string message, Color colour, LogLevel logLevel)
        {
            Instance.Log(message, StackTraceUtility.ExtractStackTrace(), colour, logLevel);
        }

        private void Log(string message, string callstack, Color colour, LogLevel logLevel)
        {
            if (hudMessages.CanAllocate(1))
            {
                GameObject messageGameObject = hudMessages.Allocate();
                HudMessage hudMessage = messageGameObject.GetComponent<HudMessage>();
                hudMessage.SetUp(message, colour, () => { hudMessages.Deallocate(messageGameObject); });
                messageGameObject.SetActive(true);
            }
            
            if (logMessages != null)
            {
                logMessages.AddItem(new LogMessage() { message = message, trackTrace = callstack, logType = logLevel });
            }
        }

        #endregion
    }
}
