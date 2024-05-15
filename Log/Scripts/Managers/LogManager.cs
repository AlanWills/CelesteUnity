using UnityEngine;

namespace Celeste.Log
{
    [AddComponentMenu("Celeste/Log/Log Manager")]
    public class LogManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private LogRecord logRecord;
        [SerializeField] private SectionLogSettingsCatalogue sectionLogSettingsCatalogue;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (logRecord != null && sectionLogSettingsCatalogue != null)
            {
                logRecord.Initialize(UnityEngine.Debug.unityLogger.logHandler, sectionLogSettingsCatalogue);
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

        #endregion
    }
}