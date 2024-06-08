using Celeste.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Log
{
    [CreateAssetMenu(fileName = nameof(LogRecord), menuName = CelesteMenuItemConstants.LOG_MENU_ITEM + "Log Record", order = CelesteMenuItemConstants.LOG_MENU_ITEM_PRIORITY)]
    public class LogRecord : ScriptableObject, ILogHandler
    {
        #region Properties and Fields

        public int NumSectionLogSettings => sectionLogSettingsCatalogue.NumItems;

        [NonSerialized] private ILogHandler defaultUnityLogHandler;
        [NonSerialized] private ICustomLogHandler hudLogHandler;
        [NonSerialized] private List<ICustomLogHandler> customLogHandlers = new List<ICustomLogHandler>();
        [NonSerialized] private HashSet<SectionLogSettings> blacklistedSections = new HashSet<SectionLogSettings>();
        [NonSerialized] private SectionLogSettingsCatalogue sectionLogSettingsCatalogue;
        [NonSerialized] private Semaphore loggingException = new Semaphore();
        [NonSerialized] private Semaphore loggingNormally = new Semaphore();

        #endregion

        public void Initialize(ILogHandler _defaultUnityLogHandler, SectionLogSettingsCatalogue _sectionLogSettingsCatalogue)
        {
            defaultUnityLogHandler = _defaultUnityLogHandler;
            hudLogHandler = new HudLogHandler();
            sectionLogSettingsCatalogue = _sectionLogSettingsCatalogue;
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
                if (context is SectionLogSettings logSettings)
                {
                    if (!blacklistedSections.Contains(logSettings))
                    {
                        string formattedException = logSettings.FormatException(exception);
                        defaultUnityLogHandler.LogFormat(LogType.Exception, logSettings.LogContext, "{0}", formattedException);

                        if (logSettings.ShouldLogToHud(LogType.Exception))
                        {
                            hudLogHandler.LogException(exception, logSettings.LogContext, formattedException);
                        }

                        for (int i = 0, n = customLogHandlers.Count; i < n; ++i)
                        {
                            customLogHandlers[i].LogException(exception, logSettings.LogContext, formattedException);
                        }
                    }
                }
                else
                {
                    string exceptionMessage = $"{exception.Message}";
                    defaultUnityLogHandler.LogException(exception, context);
                    hudLogHandler.LogException(exception, context, exceptionMessage);

                    for (int i = 0, n = customLogHandlers.Count; i < n; ++i)
                    {
                        customLogHandlers[i].LogException(exception, context, exceptionMessage);
                    }
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
                if (context is SectionLogSettings logSettings)
                {
                    string formattedLog = logSettings.FormatLogMessage(format, args);
                    defaultUnityLogHandler.LogFormat(logType, logSettings.LogContext, "{0}", formattedLog);

                    if (logSettings.ShouldLogToHud(logType))
                    {
                        hudLogHandler.Log(logType, logSettings.LogContext, formattedLog);
                    }

                    for (int i = 0, n = customLogHandlers.Count; i < n; ++i)
                    {
                        customLogHandlers[i].Log(logType, logSettings.LogContext, formattedLog);
                    }
                }
                else
                {
                    string formattedLog = string.Format(format, args);
                    defaultUnityLogHandler.LogFormat(logType, context, format, args);
                    hudLogHandler.Log(logType, context, formattedLog);

                    for (int i = 0, n = customLogHandlers.Count; i < n; ++i)
                    {
                        customLogHandlers[i].Log(logType, context, formattedLog);
                    }
                }
            }
        }
    }
}