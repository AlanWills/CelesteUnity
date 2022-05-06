using Celeste.Debug.Menus;
using Celeste.Objects.Types;
using Celeste.Tools;
using System;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Celeste.Log
{
    [CreateAssetMenu(fileName = nameof(HudLogDebugMenu), menuName = "Celeste/Log/Debug/Hud Log Debug Menu")]
    public class HudLogDebugMenu : DebugMenu
    {
        #region Properties and Fields
        
        [SerializeField] private StringList logMessages;

        [NonSerialized] private int currentPage = 0;

        private const int ENTRIES_PER_PAGE = 20;

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

            if (logMessages != null)
            {
                currentPage = GUIUtils.ReadOnlyPaginatedList(
                    currentPage,
                    ENTRIES_PER_PAGE,
                    logMessages.NumItems,
                    (i) => Label(logMessages.GetItem(i)));
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