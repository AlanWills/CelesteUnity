using Celeste.Localisation.Settings;
using CelesteEditor.Tools.Settings;
using UnityEditor;

namespace CelesteEditor.Localisation.Settings
{
    public class LocalisationEditorSettingsProvider : EditorSettingsProvider<LocalisationEditorSettings>
    {
        public LocalisationEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(LocalisationEditorSettings.GetOrCreateSettings(), path, scope) { }

        [SettingsProvider]
        public static SettingsProvider CreateLocalisationSettingsProvider()
        {
            return new LocalisationEditorSettingsProvider("Project/Celeste/Localisation Settings", SettingsScope.Project)
            {
                keywords = GetSearchKeywordsFromPath(LocalisationEditorSettings.FILE_PATH)
            };
        }
    }
}