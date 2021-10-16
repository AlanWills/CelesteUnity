using Celeste.Debug.Menus;
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

            int numPages = Mathf.CeilToInt((float)logEntries.Count / ENTRIES_PER_PAGE);
            
            using (HorizontalScope horizontal = new HorizontalScope())
            {
                // Fast Back
                {
                    GUI.enabled = currentPage > 0;
                    if (Button("<<", ExpandWidth(false)))
                    {
                        currentPage = Mathf.Max(0, currentPage - 5);
                    }
                }

                // Back
                {
                    GUI.enabled = currentPage > 0;
                    if (Button("<", ExpandWidth(false)))
                    {
                        currentPage = Mathf.Max(0, currentPage - 1);
                    }
                }

                FlexibleSpace();
                Label($"{currentPage + 1} / {numPages}", ExpandWidth(false));
                FlexibleSpace();

                // Forward
                {
                    GUI.enabled = currentPage < numPages - 1;
                    if (Button(">", ExpandWidth(false)))
                    {
                        currentPage = Mathf.Min(numPages, currentPage + 1);
                    }
                }

                // Fast Forward
                {
                    GUI.enabled = currentPage < numPages - 1;
                    if (Button(">>", ExpandWidth(false)))
                    {
                        currentPage = Mathf.Min(numPages, currentPage + 5);
                    }
                }
            }


            for (int i = 0; i < Mathf.Min(ENTRIES_PER_PAGE, logEntries.Count); ++i)
            {
                int startingIndex = currentPage * ENTRIES_PER_PAGE;
                Label(logEntries[startingIndex + i]);
            }
        }
    }
}