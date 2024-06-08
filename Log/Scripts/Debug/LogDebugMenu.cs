using Celeste.Debug.Menus;
using UnityEngine;

namespace Celeste.Log.Debug
{
    [CreateAssetMenu(fileName = nameof(LogDebugMenu), menuName = CelesteMenuItemConstants.LOG_MENU_ITEM + "Debug/Log Debug Menu", order = CelesteMenuItemConstants.LOG_MENU_ITEM_PRIORITY)]
    public class LogDebugMenu : DebugMenu
    {
        [SerializeField] private LogRecord logRecord;

        protected override void OnDrawMenu()
        {
            for (int i = 0, n = logRecord.NumSectionLogSettings; i < n; ++i)
            {
                SectionLogSettings sectionLogSettings = logRecord.GetSectionLogSettings(i);
                bool wasBlacklisted = logRecord.IsSectionBlacklisted(sectionLogSettings);
                bool isBlacklisted = GUILayout.Toggle(sectionLogSettings.SectionName, wasBlacklisted);

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
        }
    }
}