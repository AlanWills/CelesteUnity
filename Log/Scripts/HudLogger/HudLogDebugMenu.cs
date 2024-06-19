using Celeste.Debug.Menus;
using Celeste.Log.DataStructures;
using Celeste.Tools;
using System;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Celeste.Log
{
    [CreateAssetMenu(fileName = nameof(HudLogDebugMenu), menuName = CelesteMenuItemConstants.LOG_MENU_ITEM + "Debug/Hud Log Debug Menu", order = CelesteMenuItemConstants.LOG_MENU_ITEM_PRIORITY)]
    public class HudLogDebugMenu : DebugMenu
    {
        #region Properties and Fields
        
        [SerializeField] private LogMessageList logMessages;

        [NonSerialized] private int currentlyExpanded = NOT_EXPANDED;
        [NonSerialized] private int currentPage = 0;
        [NonSerialized] private LogLevel currentlyShowingLevel = LogLevel.Assert | LogLevel.Exception | LogLevel.Error | LogLevel.Warning | LogLevel.Info;
        [NonSerialized] private string logFilter;

        private const int ENTRIES_PER_PAGE = 20;
        private const int NOT_EXPANDED = -1;

        #endregion

        protected override void OnDrawMenu()
        {
            if (Button("Clear"))
            {
                HudLog.Clear();
            }

            Space(5);
            Label("Allow These Log Levels:", CelesteGUIStyles.CentredBoldLabel);

            using (new HorizontalScope())
            {
                DrawLogLevelModifier(LogLevel.Info);
                DrawLogLevelModifier(LogLevel.Warning);
                DrawLogLevelModifier(LogLevel.Error);
            }

            using (new HorizontalScope())
            {
                DrawLogLevelModifier(LogLevel.Exception);
                DrawLogLevelModifier(LogLevel.Assert);
            }

            if (logMessages != null)
            {
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

                Space(5);
                currentPage = GUIExtensions.ReadOnlyPaginatedList(
                    currentPage,
                    ENTRIES_PER_PAGE,
                    logMessages.NumItems,
                    (i) =>
                    {
                        var logMessage = logMessages.GetItem(i);

                        if (Button(logMessage.message, CelesteGUIStyles.WrappedButton.New().Alignment(TextAnchor.MiddleLeft), ExpandWidth(true)))
                        {
                            currentlyExpanded = currentlyExpanded == NOT_EXPANDED ? i : NOT_EXPANDED;
                        }

                        if (currentlyExpanded == i)
                        {
                            Label(logMessage.stackTrace, CelesteGUIStyles.WrappedLabel.New().FontSize(10).Alignment(TextAnchor.MiddleLeft));
                        }
                    },
                    (i) =>
                    {
                        var logMessage = logMessages.GetItem(i);
                        
                        if (!currentlyShowingLevel.HasFlag(logMessage.logType))
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
        }

        private void DrawLogLevelModifier(LogLevel hudLogLevel)
        {
            bool isEnabled = HudLog.IsLogLevelEnabled(hudLogLevel);
            if (isEnabled != Toggle(isEnabled, hudLogLevel.ToString()))
            {
                if (isEnabled)
                {
                    // Was enabled and is now not
                    HudLog.RemoveLogLevel(hudLogLevel);
                }
                else
                {
                    // Was not enabled and now is
                    HudLog.AddLogLevel(hudLogLevel);
                }
            }
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