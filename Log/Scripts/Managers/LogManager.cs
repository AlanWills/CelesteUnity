using Celeste.Persistence;
using Celeste.RemoteConfig;
using Celeste.Tools;
using System;
using UnityEngine;

namespace Celeste.Log
{
    [AddComponentMenu("Celeste/Log/Log Manager")]
    public class LogManager : PersistentSceneManager<LogManager, LogManagerDTO>
    {
        #region Properties and Fields

        public const string FILE_NAME = "Log.dat";
        protected override string FileName => FILE_NAME;

        public SectionLogSettingsCatalogue SectionLogSettingsCatalogue
        {
            set
            {
                if (sectionLogSettingsCatalogue != value)
                {
                    sectionLogSettingsCatalogue = value;
                    EditorOnly.SetDirty(this);
                }
            }
        }

        [SerializeField] private LogRecord logRecord;
        [SerializeField] private SectionLogSettingsCatalogue sectionLogSettingsCatalogue;
        [SerializeField] private RemoteConfigRecord remoteConfigRecord;
        [SerializeField] private LogLevel defaultHudLogLevel = LogLevel.Assert | LogLevel.Exception | LogLevel.Error | LogLevel.Warning;

        [NonSerialized] private ILogHandler unityLogHandler;

        private const string LOG_CONFIG_KEY = "LogConfig";

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (logRecord != null && sectionLogSettingsCatalogue != null)
            {
                unityLogHandler = UnityEngine.Debug.unityLogger.logHandler;
                
                if (logRecord.Equals(unityLogHandler))
                {
                    UnityEngine.Debug.LogAssertion($"Our custom {nameof(LogRecord)} is about to be used as the unity default log handler.  This is going to cause Stack Overflows so will be replaced with the UnityEngine.DebugLogHandler.");
                    unityLogHandler = Activator.CreateInstance("UnityEngine", "DebugLogHandler") as ILogHandler;
                }

                logRecord.Initialize(unityLogHandler, sectionLogSettingsCatalogue);
                UnityEngine.Debug.unityLogger.logHandler = logRecord;
                
                SyncLogSettingsFromRemoteConfig();
            }
            else if (logRecord == null)
            {
                UnityEngine.Debug.LogWarning($"{nameof(LogManager)} will not activate custom log behaviour due to unassigned {nameof(logRecord)} in {name}.", CelesteLog.Core.WithContext(this));
            }
            else if (sectionLogSettingsCatalogue == null)
            {
                UnityEngine.Debug.LogWarning($"{nameof(LogManager)} will not activate custom log behaviour due to unassigned {nameof(sectionLogSettingsCatalogue)} in {name}.", CelesteLog.Core.WithContext(this));
            }
        }

        private void OnDisable()
        {
            if (logRecord.Equals(unityLogHandler))
            {
                UnityEngine.Debug.LogAssertion($"The default unity log handler looks odd, so we're going to set it to null.  Unity may not recover from this in which case, you may lose logs until you restart Unity.");
                unityLogHandler = Activator.CreateInstance("UnityEngine", "DebugLogHandler") as ILogHandler;
            }
            else if (unityLogHandler != null)
            {
                UnityEngine.Debug.unityLogger.logHandler = unityLogHandler;
            }
        }

        #endregion

        #region Save/Load

        protected override LogManagerDTO Serialize()
        {
            return new LogManagerDTO()
            {
                currentHudLogLevel = (int)HudLog.GetAllEnabledLogLevels()
            };
        }

        protected override void Deserialize(LogManagerDTO dto)
        {
            HudLog.Hookup(Save, (LogLevel)dto.currentHudLogLevel);
        }

        protected override void SetDefaultValues()
        {
            HudLog.Hookup(Save, defaultHudLogLevel);
        }

        #endregion

        private void SyncLogSettingsFromRemoteConfig()
        {
            if (remoteConfigRecord == null)
            {
                return;
            }

            IRemoteConfigDictionary logConfig = remoteConfigRecord.GetDictionary(LOG_CONFIG_KEY);

            if (logConfig != null)
            {
                for (int i = 0, n = logRecord.NumSectionLogSettings; i < n; ++i)
                {
                    SectionLogSettings sectionLogSettings = logRecord.GetSectionLogSettings(i);
                    string sectionSettings = remoteConfigRecord.GetString(sectionLogSettings.SectionName, string.Empty);

                    if (!string.IsNullOrEmpty(sectionSettings))
                    {
                        JsonUtility.FromJsonOverwrite(sectionSettings, sectionSettings);
                    }
                }
            }
        }

        #region Callbacks

        public void OnRemoteConfigChanged()
        {
            SyncLogSettingsFromRemoteConfig();
        }

        #endregion
    }
}