﻿using Celeste.Debug.Menus;
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
        
        [SerializeField] private HudLogMessageList logMessages;

        [NonSerialized] private int currentlyExpanded = NOT_EXPANDED;
        [NonSerialized] private int currentPage = 0;

        private const int ENTRIES_PER_PAGE = 20;
        private const int NOT_EXPANDED = -1;

        #endregion

        protected override void OnDrawMenu()
        {
            if (Button("Clear"))
            {
                HudLog.Clear();
            }

            DrawHudLogLevel(HudLogLevel.Info);
            DrawHudLogLevel(HudLogLevel.Warning);
            DrawHudLogLevel(HudLogLevel.Error);
            DrawHudLogLevel(HudLogLevel.Exception);
            DrawHudLogLevel(HudLogLevel.Assert);

            if (logMessages != null)
            {
                currentPage = GUIExtensions.ReadOnlyPaginatedList(
                    currentPage,
                    ENTRIES_PER_PAGE,
                    logMessages.NumItems,
                    (i) =>
                    {
                        var logMessage = logMessages.GetItem(i);

                        if (Button(logMessage.message))
                        {
                            currentlyExpanded = currentlyExpanded == NOT_EXPANDED ? i : NOT_EXPANDED;
                        }

                        if (currentlyExpanded == i)
                        {
                            Label(logMessage.callstack);
                        }
                    });
            }
        }

        private void DrawHudLogLevel(HudLogLevel hudLogLevel)
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
    }
}