using Celeste.Debug.Menus;
using Celeste.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Celeste.Log
{
    [CreateAssetMenu(fileName = nameof(HudLogDebugMenu), menuName = "Celeste/Log/Debug/Hud Log Debug Menu")]
    public class HudLogDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [NonSerialized] private int currentPage = 0;
        [NonSerialized] private List<string> logEntries = new List<string>();

        private const int ENTRIES_PER_PAGE = 20;

        #endregion

        public void AddLogEntry(string logMessage)
        {
            logEntries.Add(logMessage);
        }

        protected override void OnDrawMenu()
        {
            if (Button("Clear"))
            {
                logEntries.Clear();
            }

            currentPage = GUIUtils.ReadOnlyPaginatedList(
                currentPage,
                ENTRIES_PER_PAGE,
                logEntries.Count,
                (i) => Label(logEntries[i]));
        }
    }
}