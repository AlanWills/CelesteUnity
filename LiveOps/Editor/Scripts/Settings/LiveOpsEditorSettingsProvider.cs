using Celeste.LiveOps.Settings;
using CelesteEditor.Tools.Settings;
using UnityEditor;

namespace CelesteEditor.LiveOps.Settings
{
    public class LiveOpsEditorSettingsProvider : EditorSettingsProvider<LiveOpsEditorSettings>
    {
        public LiveOpsEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(LiveOpsEditorSettings.GetOrCreateSettings(), path, scope) { }

        [SettingsProvider]
        public static SettingsProvider CreateLiveOpsSettingsProvider()
        {
            return new LiveOpsEditorSettingsProvider("Project/Celeste/Live Ops Settings", SettingsScope.Project)
            {
                keywords = GetSearchKeywordsFromPath(LiveOpsEditorSettings.FILE_PATH)
            };
        }
    }
}