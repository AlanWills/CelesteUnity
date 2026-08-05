using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Celeste.Events;
using Cysharp.Threading.Tasks;
using QC.Match;
using UnityEngine;
using Semaphore = Celeste.Tools.Semaphore;

namespace Celeste.Log
{
    [CreateAssetMenu(fileName = nameof(LogRecord), menuName = CelesteMenuItemConstants.LOG_MENU_ITEM + "Log Record", order = CelesteMenuItemConstants.LOG_MENU_ITEM_PRIORITY)]
    public class LogRecord : ScriptableObject, ILogHandler
    {
        #region Properties and Fields

        public int NumSectionLogSettings => sectionLogSettingsCatalogue.NumItems;
        public IReadOnlyList<LogMessage> LogMessages => logMessages;
        public int StackFramesToDiscard { get; set; }

        [SerializeField] private BoolValue isDebugBuild;
        [SerializeField] private int defaultStackFramesToDiscard = 0;

        [NonSerialized] private bool runtimeIsDebugBuild;
        [NonSerialized] private ILogHandler defaultUnityLogHandler;
        [NonSerialized] private ICustomLogHandler hudLogHandler;
        [NonSerialized] private List<ICustomLogHandler> customLogHandlers = new List<ICustomLogHandler>();
        [NonSerialized] private HashSet<SectionLogSettings> blacklistedSections = new HashSet<SectionLogSettings>();
        [NonSerialized] private SectionLogSettingsCatalogue sectionLogSettingsCatalogue;
        [NonSerialized] private Semaphore loggingException = new Semaphore();
        [NonSerialized] private Semaphore loggingNormally = new Semaphore();
        [NonSerialized] private List<LogMessage> logMessages = new List<LogMessage>();
        
        [NonSerialized] private Channel<RawUnityLog> logMessagesChannel;

        #endregion

        public void Initialize(ILogHandler _defaultUnityLogHandler, SectionLogSettingsCatalogue _sectionLogSettingsCatalogue)
        {
            runtimeIsDebugBuild = isDebugBuild.Value;
            defaultUnityLogHandler = _defaultUnityLogHandler;
            hudLogHandler = new HudLogHandler();
            sectionLogSettingsCatalogue = _sectionLogSettingsCatalogue;
            StackFramesToDiscard = defaultStackFramesToDiscard;

            logMessages.Clear();
            logMessagesChannel = Channel.CreateSingleConsumerUnbounded<RawUnityLog>();
            
            isDebugBuild.AddValueChangedCallback(OnIsDebugBuildValueChanged);
            
            ProcessLogsAsync().Forget();
        }

        public void Shutdown()
        {
            isDebugBuild.RemoveValueChangedCallback(OnIsDebugBuildValueChanged);
            
            logMessagesChannel?.Writer.TryComplete();
        }

        [HideInCallstack]
        private async UniTask ProcessLogsAsync()
        {
            await UniTask.SwitchToMainThread();
            
            await foreach (var nextLog in logMessagesChannel.Reader.ReadAllAsync())
            {
                if (nextLog.logType == LogType.Exception)
                {
                    HandleLogException(nextLog);
                }
                else
                {
                    HandleLog(nextLog);
                }
            }
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

        [HideInCallstack]
        public void LogException(Exception exception, UnityEngine.Object context)
        {
            if (loggingException.Locked)
            {
                // Prevent infinite loops
                return;
            }

            logMessagesChannel.Writer.TryWrite(new RawUnityLog
            {
                logType = LogType.Exception,
                context = context,
                format = "{0}",
                args = new object[] { exception }
            });
        }

        [HideInCallstack]
        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            if (loggingNormally.Locked)
            {
                // Prevent infinite loops
                return;
            }

            logMessagesChannel.Writer.TryWrite(new RawUnityLog
            {
                logType = logType,
                context = context,
                format = format,
                args = args
            });
        }

        public void Clear()
        {
            logMessages.Clear();
        }

        private void TrackLogMessage(string message, string stackTrace, LogLevel logLevel, SectionLogSettings sectionLogSettings)
        {
            if (runtimeIsDebugBuild)
            {
                logMessages.Add(new LogMessage
                {
                    message = message,
                    stackTrace = stackTrace,
                    logType = logLevel,
                    sectionLogSettings = sectionLogSettings
                });
            }
        }
        
        [HideInCallstack]
        private void HandleLogException(RawUnityLog rawUnityLog)
        {
            using (loggingException.Lock())
            {
                UnityEngine.Object context = rawUnityLog.context;
                Exception exception = rawUnityLog.args[0] as Exception;
                string formattedException;

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

        [HideInCallstack]
        private void HandleLog(RawUnityLog rawUnityLog)
        {
            using (loggingNormally.Lock())
            {
                UnityEngine.Object context = rawUnityLog.context;
                string format = rawUnityLog.format;
                LogType logType = rawUnityLog.logType;
                object[] args = rawUnityLog.args;
                
                StackTrace stackTrace = new StackTrace(StackFramesToDiscard, true);
                string stackTraceString = stackTrace.ToString();
                string formattedLog;

                if (context is SectionLogSettings logSettings)
                {
                    if (!blacklistedSections.Contains(logSettings))
                    {
                        formattedLog = logSettings.FormatLogMessage(format, args);
                        defaultUnityLogHandler.LogFormat(logType, logSettings.LogContext, "{0}", formattedLog);

                        if (logSettings.ShouldLogToHud(logType))
                        {
                            hudLogHandler.Log(logType, logSettings.LogContext, formattedLog, stackTraceString);
                        }

                        for (int i = 0, n = customLogHandlers.Count; i < n; ++i)
                        {
                            customLogHandlers[i].Log(logType, logSettings.LogContext, formattedLog, stackTraceString);
                        }

                        TrackLogMessage(formattedLog, stackTraceString, logType.ToLogLevel(), logSettings);
                    }
                }
                else
                {
                    formattedLog = string.Format(format, args);
                    defaultUnityLogHandler.LogFormat(logType, context, format, args);
                    hudLogHandler.Log(logType, context, formattedLog, stackTraceString);

                    for (int i = 0, n = customLogHandlers.Count; i < n; ++i)
                    {
                        customLogHandlers[i].Log(logType, context, formattedLog, stackTraceString);
                    }

                    TrackLogMessage(formattedLog, stackTraceString, logType.ToLogLevel(), null);
                }
            }
        }
        
        #region Callbacks

        private void OnIsDebugBuildValueChanged(ValueChangedArgs<bool> args)
        {
            runtimeIsDebugBuild = args.newValue;
        }
        
        #endregion
    }
}