using Celeste.DataImporters.Settings;
using CelesteEditor.Tools.Settings;
using UnityEditor;

namespace CelesteEditor.DataImporters.Settings
{
    public class DataImporterEditorSettingsProvider : EditorSettingsProvider<DataImporterEditorSettings>
    {
        public DataImporterEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(DataImporterEditorSettings.GetOrCreateSettings(), path, scope) { }

        [SettingsProvider]
        public static SettingsProvider CreateDataImporterSettingsProvider()
        {
            return new DataImporterEditorSettingsProvider("Project/Celeste/Data Importer Settings", SettingsScope.Project)
            {
                keywords = GetSearchKeywordsFromPath(DataImporterEditorSettings.FILE_PATH)
            };
        }
    }
}
