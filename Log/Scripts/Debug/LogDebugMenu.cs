using Celeste.Debug.Menus;
using Celeste.Tools;
using System;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Celeste.Log.Debug
{
    [CreateAssetMenu(fileName = nameof(LogDebugMenu), menuName = CelesteMenuItemConstants.LOG_MENU_ITEM + "Debug/Log Debug Menu", order = CelesteMenuItemConstants.LOG_MENU_ITEM_PRIORITY)]
    public class LogDebugMenu : DebugMenu
    {
        [SerializeField] private LogRecord logRecord;
        
        [NonSerialized] private int currentlyExpanded = NOT_EXPANDED;
        [NonSerialized] private int currentPage = 0;
        [NonSerialized] private LogLevel currentlyShowingLevel = LogLevel.Assert | LogLevel.Exception | LogLevel.Error | LogLevel.Warning | LogLevel.Info;
        [NonSerialized] private string logFilter;
        [NonSerialized] private GUIStyle buttonStyle;
        [NonSerialized] private GUIStyle stackTraceStyle;
        [NonSerialized] private int maxMessageLength = 100;

        private const int ENTRIES_PER_PAGE = 20;
        private const int NOT_EXPANDED = -1;

        protected override void OnShowMenu()
        {
            base.OnShowMenu();

            buttonStyle = CelesteGUIStyles.WrappedButton.New().Alignment(TextAnchor.MiddleLeft);
            stackTraceStyle = CelesteGUIStyles.WrappedLabel.New().FontSize(10).Alignment(TextAnchor.MiddleLeft);
        }

        protected override void OnDrawMenu()
        {
            if (Button("Clear"))
            {
                logRecord.Clear();
            }

            Space(5);

            for (int i = 0, n = logRecord.NumSectionLogSettings; i < n; ++i)
            {
                SectionLogSettings sectionLogSettings = logRecord.GetSectionLogSettings(i);
                bool wasBlacklisted = logRecord.IsSectionBlacklisted(sectionLogSettings);
                bool isBlacklisted = Toggle(wasBlacklisted, $"Is {sectionLogSettings.SectionName} Blacklisted");

                if (wasBlacklisted != isBlacklisted)
                {
                    if (isBlacklisted)
                    {
                        logRecord.AddSectionToBlacklist(sectionLogSettings);
                    }
                    else
                    {
                        logRecord.RemoveSectionFromBlacklist(sectionLogSettings);
                    }
                }
            }

            Space(5);
            Label("Show These Log Levels Here:", CelesteGUIStyles.CentredBoldLabel);

            using (new HorizontalScope())
            {
                DrawLogLevelFilter(LogLevel.Info);
                DrawLogLevelFilter(LogLevel.Warning);
                DrawLogLevelFilter(LogLevel.Error);
            }

            using (new HorizontalScope())
            {
                DrawLogLevelFilter(LogLevel.Exception);
                DrawLogLevelFilter(LogLevel.Assert);
            }

            using (new HorizontalScope())
            {
                Space(5);
                Label("Log Filter");
                logFilter = TextField(logFilter);
            }

            using (new HorizontalScope())
            {
                Space(5);
                maxMessageLength = GUIExtensions.IntField("Max Message Length", maxMessageLength);
            }

            Space(5);
            currentPage = GUIExtensions.ReadOnlyPaginatedList(
                currentPage,
                ENTRIES_PER_PAGE,
                logRecord.LogMessages.Count,
                (i) =>
                {
                    var logMessage = logRecord.LogMessages[i];
                    string message = logMessage.message;

                    if (message.Length > maxMessageLength)
                    {
                        message = message.Substring(0, maxMessageLength);
                    }

                    if (Button(message, buttonStyle, ExpandWidth(true)))
                    {
                        currentlyExpanded = currentlyExpanded == NOT_EXPANDED ? i : NOT_EXPANDED;
                    }

                    if (currentlyExpanded == i)
                    {
                        Label(logMessage.stackTrace, stackTraceStyle);
                    }
                },
                (i) =>
                {
                    var logMessage = logRecord.LogMessages[i];

                    if (!currentlyShowingLevel.HasFlag(logMessage.logType))
                    {
                        return false;
                    }

                    if (logMessage.sectionLogSettings != null && logRecord.IsSectionBlacklisted(logMessage.sectionLogSettings))
                    {
                        return false;
                    }

                    if (!string.IsNullOrEmpty(logFilter))
                    {
                        return logMessage.message.Contains(logFilter, StringComparison.OrdinalIgnoreCase) ||
                               logMessage.stackTrace.Contains(logFilter, StringComparison.OrdinalIgnoreCase);
                    }

                    return true;
                });
        }

        private void DrawLogLevelFilter(LogLevel hudLogLevel)
        {
            bool isEnabled = currentlyShowingLevel.HasFlag(hudLogLevel);
            if (isEnabled != Toggle(isEnabled, hudLogLevel.ToString(), ExpandWidth(false)))
            {
                if (isEnabled)
                {
                    // Was enabled and is now not
                    currentlyShowingLevel &= ~hudLogLevel;
                }
                else
                {
                    // Was not enabled and now is
                    currentlyShowingLevel |= hudLogLevel;
                }
            }
        }
    }
}