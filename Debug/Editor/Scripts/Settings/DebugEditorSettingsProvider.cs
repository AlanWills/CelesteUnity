using Celeste.Debug.Settings;
using CelesteEditor.Tools.Settings;
using UnityEditor;

namespace CelesteEditor.Debug.Settings
{
    public class DebugEditorSettingsProvider : EditorSettingsProvider<DebugEditorSettings>
    {
        public DebugEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
             : base(DebugEditorSettings.GetOrCreateSettings(), path, scope) { }

        [SettingsProvider]
        public static SettingsProvider CreateInputSettingsProvider()
        {
            return new DebugEditorSettingsProvider("Project/Celeste/Debug Settings", SettingsScope.Project)
            {
                keywords = GetSearchKeywordsFromPath(DebugEditorSettings.FILE_PATH)
            };
        }
    }
}