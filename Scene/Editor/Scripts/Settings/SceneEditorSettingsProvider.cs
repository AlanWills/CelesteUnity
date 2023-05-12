using CelesteEditor.Tools.Settings;
using UnityEditor;

namespace CelesteEditor.Scene.Settings
{
    public class SceneEditorSettingsProvider : EditorSettingsProvider<SceneEditorSettings>
    {
        public SceneEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(SceneEditorSettings.GetOrCreateSettings(), path, scope) { }

        [SettingsProvider]
        public static SettingsProvider CreateSceneSettingsProvider()
        {
            return new SceneEditorSettingsProvider("Project/Celeste/Scene Settings", SettingsScope.Project)
            {
                keywords = GetSearchKeywordsFromPath(SceneEditorSettings.FILE_PATH)
            };
        }
    }
}