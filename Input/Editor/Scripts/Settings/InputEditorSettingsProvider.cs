using Celeste.Input.Settings;
using CelesteEditor.Tools.Settings;
using UnityEditor;

namespace CelesteEditor.Input.Settings
{
    public class InputEditorSettingsProvider : EditorSettingsProvider<InputEditorSettings>
    {
        public InputEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(InputEditorSettings.GetOrCreateSettings(), path, scope) { }

        [SettingsProvider]
        public static SettingsProvider CreateInputSettingsProvider()
        {
            return new InputEditorSettingsProvider("Project/Celeste/Input Settings", SettingsScope.Project)
            {
                keywords = GetSearchKeywordsFromPath(InputEditorSettings.FILE_PATH)
            };
        }
    }
}