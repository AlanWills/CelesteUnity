using Celeste.RemoteConfig;
using Celeste.Tools;
using System;
using Celeste.DataStructures;
using UnityEngine;

namespace Celeste.Log
{
    [AddComponentMenu("Celeste/Log/Log Manager")]
    public class LogManager : MonoBehaviour
    {
        #region Properties and Fields

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
                    //unityLogHandler = Activator.CreateInstance("UnityEngine", "DebugLogHandler") as ILogHandler;
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
                //unityLogHandler = Activator.CreateInstance("UnityEngine", "DebugLogHandler") as ILogHandler;
            }
            else if (unityLogHandler != null)
            {
                UnityEngine.Debug.unityLogger.logHandler = unityLogHandler;
            }
        }

        #endregion

        private void SyncLogSettingsFromRemoteConfig()
        {
            if (remoteConfigRecord == null)
            {
                return;
            }

            IDataDictionary logConfig = remoteConfigRecord.GetObjectAsDictionary(LOG_CONFIG_KEY);
            if (logConfig == null)
            {
                return;
            }
            
            logRecord.StackFramesToDiscard = logConfig.GetInt(nameof(logRecord.StackFramesToDiscard), logRecord.StackFramesToDiscard);
                
            for (int i = 0, n = logRecord.NumSectionLogSettings; i < n; ++i)
            {
                SectionLogSettings sectionLogSettings = logRecord.GetSectionLogSettings(i);
                string sectionSettings = logConfig.GetString(sectionLogSettings.SectionName, string.Empty);

                if (!string.IsNullOrEmpty(sectionSettings))
                {
                    JsonUtility.FromJsonOverwrite(sectionSettings, sectionSettings);
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