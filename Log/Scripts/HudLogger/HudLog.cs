using Celeste.Log.DataStructures;
using Celeste.Memory;
using Celeste.Objects;
using System;
using UnityEngine;

namespace Celeste.Log
{
    [Serializable, Flags]
    public enum HudLogLevel
    {
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 4,
        Exception = 8,
        Assert = 16,
    }

    [AddComponentMenu("Celeste/Log/Hud Log")]
    public class HudLog : SceneSingleton<HudLog>
    {
        #region Properties and Fields

        private HudLogLevel LogLevel
        {
            get
            {
                if (!logLevelInitialized)
                {
                    logLevel = (HudLogLevel)PlayerPrefs.GetInt(HUD_LOG_LEVEL_PREFS_KEY, (int)defaultLogLevel);
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

        [SerializeField] private HudLogLevel defaultLogLevel = HudLogLevel.Assert | HudLogLevel.Exception | HudLogLevel.Error | HudLogLevel.Warning | HudLogLevel.Info;
        [SerializeField] private HudLogMessageList logMessages;
        [SerializeField] private GameObjectAllocator hudMessages;
        [SerializeField] private Color infoColour = Color.white;
        [SerializeField] private Color warningColour = Color.yellow;
        [SerializeField] private Color errorColour = Color.red;

        private HudLogLevel logLevel;
        private bool logLevelInitialized;

        private const string HUD_LOG_LEVEL_PREFS_KEY = "HudLogLevel";

        #endregion

        #region Log Type

        public static bool IsLogLevelEnabled(HudLogLevel desiredLevel)
        {
            return Instance != null && (Instance.LogLevel & desiredLevel) == desiredLevel;
        }

        public static void AddLogLevel(HudLogLevel desiredLevel)
        {
            Instance.LogLevel |= desiredLevel;
        }

        public static void RemoveLogLevel(HudLogLevel desiredLevel)
        {
            Instance.LogLevel &= ~desiredLevel;
        }

        #endregion

        #region Logging Methods

        public static void LogInfo(string message)
        {
            if (Instance != null && IsLogLevelEnabled(HudLogLevel.Info))
            {
                Instance.Log(message, Instance.infoColour);
                UnityEngine.Debug.Log(message);
            }
        }

        public static void LogWarning(string message)
        {
            if (Instance != null && IsLogLevelEnabled(HudLogLevel.Warning))
            {
                Instance.Log(message, Instance.warningColour);
                UnityEngine.Debug.LogWarning(message);
            }
        }

        public static void LogError(string message)
        {
            if (Instance != null && IsLogLevelEnabled(HudLogLevel.Error))
            {
                Instance.Log(message, Instance.errorColour);
                UnityEngine.Debug.LogError(message);
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

        private void Log(string message, Color colour)
        {
            Instance.Log(message, StackTraceUtility.ExtractStackTrace(), colour);
        }

        private void Log(string message, string callstack, Color colour)
        {
            if (hudMessages.CanAllocate(1))
            {
                GameObject messageGameObject = hudMessages.Allocate();
                HudMessage hudMessage = messageGameObject.GetComponent<HudMessage>();
                hudMessage.SetUp(message, colour, () => { hudMessages.Deallocate(messageGameObject); });
                messageGameObject.SetActive(true);
            }
            else
            {
                UnityEngine.Debug.LogWarningFormat("Hud Message limit reached.  Message: {0}", message);
            }

            if (logMessages != null)
            {
                logMessages.AddItem(new HudLogMessage() { message = message, callstack = callstack });
            }
        }

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            Application.logMessageReceived += Application_logMessageReceived;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= Application_logMessageReceived;
        }

        #endregion

        #region Callbacks

        private void Application_logMessageReceived(string logString, string stackTrace, LogType type)
        {
            if (Instance == null)
            {
                return;
            }

            if (type == LogType.Exception && IsLogLevelEnabled(HudLogLevel.Exception))
            {
                Log($"{logString}", stackTrace, Instance.errorColour);
            }
            else if (type == LogType.Assert && IsLogLevelEnabled(HudLogLevel.Assert))
            {
                Log(logString, stackTrace, Instance.errorColour);
            }
        }

        #endregion
    }
}
