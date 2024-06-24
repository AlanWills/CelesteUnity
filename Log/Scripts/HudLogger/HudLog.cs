using Celeste.CloudSave;
using Celeste.Log.DataStructures;
using Celeste.Memory;
using Celeste.Persistence;
using System;
using UnityEngine;

namespace Celeste.Log
{
    [AddComponentMenu("Celeste/Log/Hud Log")]
    public class HudLog : PersistentSceneManager<HudLog, HudLogDTO>
    {
        #region Properties and Fields

        public const string FILE_NAME = "HudLog.dat";
        protected override string FileName => FILE_NAME;

        private LogLevel LogLevel
        {
            get => logLevel;
            set
            {
                if (logLevel != value)
                {
                    logLevel = value;
                    save?.Invoke();
                }
            }
        }

        [SerializeField] private LogLevel defaultLogLevel = LogLevel.Assert | LogLevel.Exception | LogLevel.Error | LogLevel.Warning;
        [SerializeField] private LogMessageList logMessages;
        [SerializeField] private GameObjectAllocator hudMessages;
        [SerializeField] private Color infoColour = Color.white;
        [SerializeField] private Color warningColour = Color.yellow;
        [SerializeField] private Color errorColour = Color.red;

        [NonSerialized] private LogLevel logLevel;
        [NonSerialized] private Action save;
        [NonSerialized] private static HudLog instance;

        #endregion

        protected override void Awake()
        {
            UnityEngine.Debug.Assert(instance == null, $"A previous of {nameof(HudLog)} has been detected!  Are there two in the scene?");
            instance = this;

            base.Awake();
        }

        #region Save/Load

        protected override HudLogDTO Serialize()
        {
            return new HudLogDTO()
            {
                currentHudLogLevel = (int)instance.LogLevel
            };
        }

        protected override void Deserialize(HudLogDTO dto)
        {
            logLevel = (LogLevel)dto.currentHudLogLevel;
        }

        protected override void SetDefaultValues()
        {
            logLevel = defaultLogLevel;
        }

        #endregion

        #region Log Type

        public static bool IsLogLevelEnabled(LogLevel desiredLevel)
        {
            return instance != null && (instance.LogLevel & desiredLevel) == desiredLevel;
        }

        public static void AddLogLevel(LogLevel desiredLevel)
        {
            instance.LogLevel |= desiredLevel;
        }

        public static void RemoveLogLevel(LogLevel desiredLevel)
        {
            instance.LogLevel &= ~desiredLevel;
        }

        public static LogLevel GetAllEnabledLogLevels()
        {
            return instance.LogLevel;
        }

        public static void SetAllEnabledLogLevels(LogLevel enabledLogLevels)
        {
            instance.LogLevel = enabledLogLevels;
        }

        #endregion

        #region Logging Methods

        public static void LogInfo(string message)
        {
            if (instance != null && IsLogLevelEnabled(LogLevel.Info))
            {
                instance.Log(message, instance.infoColour, LogLevel.Info);
            }
        }

        public static void LogInfo(string message, string stackTrace)
        {
            if (instance != null && IsLogLevelEnabled(LogLevel.Info))
            {
                instance.Log(message, stackTrace, instance.infoColour, LogLevel.Info);
            }
        }

        public static void LogWarning(string message)
        {
            if (instance != null && IsLogLevelEnabled(LogLevel.Warning))
            {
                instance.Log(message, instance.warningColour, LogLevel.Warning);
            }
        }

        public static void LogWarning(string message, string stackTrace)
        {
            if (instance != null && IsLogLevelEnabled(LogLevel.Warning))
            {
                instance.Log(message, stackTrace, instance.warningColour, LogLevel.Warning);
            }
        }

        public static void LogError(string message)
        {
            if (instance != null && IsLogLevelEnabled(LogLevel.Error))
            {
                instance.Log(message, instance.errorColour, LogLevel.Error);
            }
        }

        public static void LogError(string message, string stackTrace)
        {
            if (instance != null && IsLogLevelEnabled(LogLevel.Error))
            {
                instance.Log(message, stackTrace, instance.errorColour, LogLevel.Error);
            }
        }

        public static void LogException(string message)
        {
            if (instance != null && IsLogLevelEnabled(LogLevel.Exception))
            {
                instance.Log(message, instance.errorColour, LogLevel.Exception);
            }
        }

        public static void LogException(string message, string stackTrace)
        {
            if (instance != null && IsLogLevelEnabled(LogLevel.Exception))
            {
                instance.Log(message, stackTrace, instance.errorColour, LogLevel.Exception);
            }
        }

        public static void LogAssertion(string message)
        {
            if (instance != null && IsLogLevelEnabled(LogLevel.Assert))
            {
                instance.Log(message, instance.errorColour, LogLevel.Assert);
            }
        }

        public static void LogAssertion(string message, string stackTrace)
        {
            if (instance != null && IsLogLevelEnabled(LogLevel.Assert))
            {
                instance.Log(message, stackTrace, instance.errorColour, LogLevel.Assert);
            }
        }

        public static void Clear()
        {
            if (instance != null)
            {
                if (instance.logMessages != null)
                {
                    instance.logMessages.ClearItems();
                }

                instance.hudMessages.DeallocateAll();
            }
        }

        private void Log(string message, Color colour, LogLevel logLevel)
        {
            instance.Log(message, StackTraceUtility.ExtractStackTrace(), colour, logLevel);
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
                logMessages.AddItem(new LogMessage() { message = message, stackTrace = callstack, logType = logLevel });
            }
        }

        #endregion

        #region Callbacks

        public void OnCloudSaveLoaded(CloudSaveLoadedArgs cloudSaveLoadedArgs)
        {
            HudLogDTO hudLogDTO = cloudSaveLoadedArgs.loadedData?.DeserializeData<HudLogDTO>(FilePath);
            
            if (hudLogDTO != null)
            {
                Deserialize(hudLogDTO);
            }
        }

        #endregion
    }
}
