using Celeste.Persistence.Settings;
using CelesteEditor.Tools.Settings;
using UnityEditor;

namespace CelesteEditor.Persistence.Settings
{
    public class PersistenceEditorSettingsProvider : EditorSettingsProvider<PersistenceEditorSettings>
    {
        public PersistenceEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(PersistenceEditorSettings.GetOrCreateSettings(), path, scope) { }

        [SettingsProvider]
        public static SettingsProvider CreatePersistenceSettingsProvider()
        {
            return new PersistenceEditorSettingsProvider("Project/Celeste/Persistence Settings", SettingsScope.Project)
            {
                keywords = GetSearchKeywordsFromPath(PersistenceEditorSettings.FILE_PATH)
            };
        }
    }
}