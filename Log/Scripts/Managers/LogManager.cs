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

        private void Awake()
        {
            if (logRecord != null && sectionLogSettingsCatalogue != null)
            {
                unityLogHandler = UnityEngine.Debug.unityLogger.logHandler;
                UnityEngine.Debug.Assert((UnityEngine.Object)unityLogHandler != logRecord, $"Our custom {nameof(LogRecord)} is about to be used as the unity default log handler.  This is going to cause Stack Overflows!");

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

        private void OnDestroy()
        {
            if (unityLogHandler != null)
            {
                UnityEngine.Debug.unityLogger.logHandler = unityLogHandler;
            }

            UnityEngine.Debug.Assert((UnityEngine.Object)UnityEngine.Debug.unityLogger.logHandler != logRecord, $"Our custom {nameof(LogRecord)} is still set up as the unity log handler!");
        }

        #endregion
    }
}