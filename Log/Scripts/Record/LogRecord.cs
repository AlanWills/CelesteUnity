using Celeste.Parameters;
using Celeste.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Log
{
    [CreateAssetMenu(fileName = nameof(LogRecord), menuName = CelesteMenuItemConstants.LOG_MENU_ITEM + "Log Record", order = CelesteMenuItemConstants.LOG_MENU_ITEM_PRIORITY)]
    public class LogRecord : ScriptableObject, ILogHandler
    {
        public struct LogData
        {
            public string message;
            public string callstack;
            public LogType logType;
        }

        #region Properties and Fields

        public int NumSectionLogSettings => sectionLogSettingsCatalogue.NumItems;
        public IReadOnlyList<LogMessage> LogMessages => logMessages;

        [SerializeField] private BoolValue isDebugBuild;

        [NonSerialized] private ILogHandler defaultUnityLogHandler;
        [NonSerialized] private ICustomLogHandler hudLogHandler;
        [NonSerialized] private List<ICustomLogHandler> customLogHandlers = new List<ICustomLogHandler>();
        [NonSerialized] private HashSet<SectionLogSettings> blacklistedSections = new HashSet<SectionLogSettings>();
        [NonSerialized] private SectionLogSettingsCatalogue sectionLogSettingsCatalogue;
        [NonSerialized] private Semaphore loggingException = new Semaphore();
        [NonSerialized] private Semaphore loggingNormally = new Semaphore();
        [NonSerialized] private List<LogMessage> logMessages = new List<LogMessage>();

        #endregion

        public void Initialize(ILogHandler _defaultUnityLogHandler, SectionLogSettingsCatalogue _sectionLogSettingsCatalogue)
        {
            defaultUnityLogHandler = _defaultUnityLogHandler;
            hudLogHandler = new HudLogHandler();
            sectionLogSettingsCatalogue = _sectionLogSettingsCatalogue;
            logMessages.Clear();
        }

        public void AddCustomLogHandler(ICustomLogHandler handler)
        {
            customLogHandlers.Add(handler);
        }

        public void RemoveCustomLogHandler<T>() where T : ICustomLogHandler
        {
            customLogHandlers.RemoveAll(x => x is T);
        }

        public void AddSectionToBlacklist(SectionLogSettings settings)
        {
            if (settings != null)
            {
                blacklistedSections.Add(settings);
            }
        }

        public void AddSectionToBlacklist(string sectionName)
        {
            SectionLogSettings settings = sectionLogSettingsCatalogue.MustFindBySectionName(sectionName);
            AddSectionToBlacklist(settings);
        }

        public void RemoveSectionFromBlacklist(SectionLogSettings settings)
        {
            if (settings != null)
            {
                blacklistedSections.Remove(settings);
            }
        }

        public SectionLogSettings GetSectionLogSettings(int index)
        {
            return sectionLogSettingsCatalogue.GetItem(index);
        }

        public bool IsSectionBlacklisted(SectionLogSettings settings)
        {
            return blacklistedSections.Contains(settings);
        }

        public void RemoveSectionFromBlacklist(string sectionName)
        {
            SectionLogSettings settings = sectionLogSettingsCatalogue.MustFindBySectionName(sectionName);
            RemoveSectionFromBlacklist(settings);
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            if (loggingException.Locked)
            {
                // Prevent infinite loops
                return;
            }

            using (loggingException.Lock())
            {
                string formattedException = string.Empty;

                if (context is SectionLogSettings logSettings)
                {
                    if (!blacklistedSections.Contains(logSettings))
                    {
                        formattedException = logSettings.FormatException(exception);
                        defaultUnityLogHandler.LogFormat(LogType.Exception, logSettings.LogContext, "{0}", formattedException);

                        if (logSettings.ShouldLogToHud(LogType.Exception))
                        {
                            hudLogHandler.LogException(exception, logSettings.LogContext, formattedException);
                        }

                        for (int i = 0, n = customLogHandlers.Count; i < n; ++i)
                        {
                            customLogHandlers[i].LogException(exception, logSettings.LogContext, formattedException);
                        }

                        TrackLogMessage(formattedException, exception.StackTrace, LogLevel.Exception, logSettings);
                    }
                }
                else
                {
                    formattedException = exception.Message;
                    defaultUnityLogHandler.LogException(exception, context);
                    hudLogHandler.LogException(exception, context, formattedException);

                    for (int i = 0, n = customLogHandlers.Count; i < n; ++i)
                    {
                        customLogHandlers[i].LogException(exception, context, formattedException);
                    }

                    TrackLogMessage(formattedException, exception.StackTrace, LogLevel.Exception, null);
                }
            }
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            if (loggingNormally.Locked)
            {
                // Prevent infinite loops
                return;
            }

            using (loggingNormally.Lock())
            {
                string stackTrace = StackTraceUtility.ExtractStackTrace();
                string formattedLog = string.Empty;

                if (context is SectionLogSettings logSettings)
                {
                    if (!blacklistedSections.Contains(logSettings))
                    {
                        formattedLog = logSettings.FormatLogMessage(format, args);
                        defaultUnityLogHandler.LogFormat(logType, logSettings.LogContext, "{0}", formattedLog);

                        if (logSettings.ShouldLogToHud(logType))
                        {
                            hudLogHandler.Log(logType, logSettings.LogContext, formattedLog, stackTrace);
                        }

                        for (int i = 0, n = customLogHandlers.Count; i < n; ++i)
                        {
                            customLogHandlers[i].Log(logType, logSettings.LogContext, formattedLog, stackTrace);
                        }

                        TrackLogMessage(formattedLog, stackTrace, logType.ToLogLevel(), logSettings);
                    }
                }
                else
                {
                    formattedLog = string.Format(format, args);
                    defaultUnityLogHandler.LogFormat(logType, context, format, args);
                    hudLogHandler.Log(logType, context, formattedLog, stackTrace);

                    for (int i = 0, n = customLogHandlers.Count; i < n; ++i)
                    {
                        customLogHandlers[i].Log(logType, context, formattedLog, stackTrace);
                    }

                    TrackLogMessage(formattedLog, stackTrace, logType.ToLogLevel(), null);
                }
            }
        }

        public void Clear()
        {
            logMessages.Clear();
        }

        private void TrackLogMessage(string message, string stackTrace, LogLevel logLevel, SectionLogSettings sectionLogSettings)
        {
            if (isDebugBuild.Value)
            {
                logMessages.Add(new LogMessage()
                {
                    message = message,
                    stackTrace = stackTrace,
                    logType = logLevel,
                    sectionLogSettings = sectionLogSettings
                });
            }
        }
    }
}