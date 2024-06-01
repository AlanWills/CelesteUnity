using System;
using UnityEngine;

namespace Celeste.Log
{
    [AddComponentMenu("Celeste/Log/Log Manager")]
    public class LogManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private LogRecord logRecord;
        [SerializeField] private SectionLogSettingsCatalogue sectionLogSettingsCatalogue;

        [NonSerialized] private ILogHandler unityLogHandler;

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
            if (unityLogHandler == null || logRecord.Equals(unityLogHandler))
            {
                UnityEngine.Debug.LogAssertion($"The default unity log handler looks odd, so we'll replace it with a fresh UnityEngine.DebugLogHandler.");
                unityLogHandler = Activator.CreateInstance("UnityEngine", "DebugLogHandler") as ILogHandler;
            }

            UnityEngine.Debug.unityLogger.logHandler = unityLogHandler;
        }

        #endregion
    }
}