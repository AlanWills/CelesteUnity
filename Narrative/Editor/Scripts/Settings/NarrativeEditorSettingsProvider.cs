using Celeste.Narrative.Settings;
using CelesteEditor.Tools.Settings;
using UnityEditor;

namespace CelesteEditor.Narrative.Settings
{
    public class NarrativeEditorSettingsProvider : EditorSettingsProvider<NarrativeEditorSettings>
    {
        private NarrativeEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(NarrativeEditorSettings.GetOrCreateSettings(), path, scope) { }

        [SettingsProvider]
        public static SettingsProvider CreateNarrativeSettingsProvider()
        {
            return new NarrativeEditorSettingsProvider("Project/Celeste/Narrative Settings")
            {
                keywords = GetSearchKeywordsFromPath(NarrativeEditorSettings.FILE_PATH)
            };
        }
    }
}